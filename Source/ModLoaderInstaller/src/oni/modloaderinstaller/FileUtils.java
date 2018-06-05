package oni.modloaderinstaller;


import java.io.IOException;
import java.math.BigInteger;
import java.net.URI;
import java.net.URISyntaxException;
import java.nio.ByteBuffer;
import java.nio.channels.FileChannel;
import java.nio.channels.SeekableByteChannel;
import java.nio.file.*;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Collections;

import static java.nio.file.StandardOpenOption.CREATE_NEW;
import static java.nio.file.StandardOpenOption.READ;
import static java.nio.file.StandardOpenOption.WRITE;
import static oni.modloaderinstaller.Constants.*;

public final class FileUtils {

	private static final int BUFFER_SIZE = 16_384;

	private FileUtils() {
		throw new Error("No instances!");
	}

	public static String hashFile(Path path) {
		if (path == null) return "";
		if (!Files.isRegularFile(path)) return "";

		try {
			MessageDigest sha1 = MessageDigest.getInstance("SHA-1");
			byte[] rawBytes = Files.readAllBytes(path);
			byte[] hash = sha1.digest(rawBytes);
			return new BigInteger(1, hash).toString(16);
		} catch (IOException | NoSuchAlgorithmException e) {
			e.printStackTrace();
		}
		return ERROR_SHA1;
	}

	public static void applyXOR(Path assemblyFile, long limit) throws URISyntaxException, IOException {
		Path inputFile = assemblyFile.resolveSibling(DLL_NAME + ".bak");
		URI xorUri = FileUtils.class.getResource("/" + XOR_NAME).toURI();




        Files.move(assemblyFile, inputFile, StandardCopyOption.REPLACE_EXISTING);

		try (FileSystem fileSystem = createFileSystem(xorUri); // Required to resolve xorUri
		     FileChannel inputChannel = FileChannel.open(inputFile, READ);
		     FileChannel xorChannel = FileChannel.open(Paths.get(xorUri), READ);
		     FileChannel outputChannel = FileChannel.open(assemblyFile, WRITE, CREATE_NEW)) {

			xorFiles(inputChannel, xorChannel, outputChannel, limit);
		} catch (Throwable t) {
			Files.deleteIfExists(assemblyFile);
			Files.move(inputFile, assemblyFile);
			throw t;
		}
	}

	private static FileSystem createFileSystem(URI uri) throws IOException {
		if ("jar".equals(uri.getScheme())) {
			return FileSystems.newFileSystem(uri, Collections.emptyMap());
		} else {
			return null;
		}
	}

	public static void xorFiles(SeekableByteChannel inputA, SeekableByteChannel inputB,
	                            SeekableByteChannel outputChannel, long limit) throws IOException {
		ByteBuffer aBuffer = ByteBuffer.allocateDirect(BUFFER_SIZE);
		ByteBuffer bBuffer = ByteBuffer.allocateDirect(BUFFER_SIZE);
		ByteBuffer outBuffer = ByteBuffer.allocateDirect(BUFFER_SIZE);

		do {
			if (inputA.position() == inputA.size()) inputA.position(0);
			if (inputB.position() == inputB.size()) inputB.position(0);

			inputA.read(aBuffer);
			inputB.read(bBuffer);
			aBuffer.flip();
			bBuffer.flip();
			int cap = min(aBuffer.remaining(), bBuffer.remaining(), outBuffer.remaining());
			for (int i = 0; i < cap; i += 8) {
				long a = aBuffer.getLong();
				long b = bBuffer.getLong();
				outBuffer.putLong(a ^ b);
			}
			aBuffer.compact();
			bBuffer.compact();

			outBuffer.flip();
			long bytesRequired = limit - outputChannel.size();
			if (outBuffer.limit() > bytesRequired) outBuffer.limit((int) bytesRequired);
			outputChannel.write(outBuffer);
			outBuffer.compact();
		} while (outputChannel.size() < limit);
	}

	// Simple 3-input min
	private static int min(int a, int b, int c) {
		return Math.min(a, Math.min(b, c));
	}
}
