package oni.modloaderinstaller;

import java.nio.channels.FileChannel;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Scanner;

import static java.nio.file.StandardOpenOption.CREATE_NEW;
import static java.nio.file.StandardOpenOption.READ;
import static java.nio.file.StandardOpenOption.WRITE;
import static oni.modloaderinstaller.Constants.DLL_NAME;
import static oni.modloaderinstaller.Constants.XOR_NAME;

public class CreatePatch {

	public static void main(String[] args) throws Exception {
		System.out.println("Verify that the original DLL is called");
		System.out.println("  " + DLL_NAME + ".orig");
		System.out.println("and that the patched DLL is called");
		System.out.println("  " + DLL_NAME);
		System.out.print("\nPath to OxygenNotIncluded_Data\\Managed: ");

		Scanner in = new Scanner(System.in);
		Path managedDir = Paths.get(in.nextLine());

		if (!Files.isDirectory(managedDir)) {
			System.out.println("Invalid directory");
			return;
		}
		System.out.print("\nPlease specify the OS - (1)linux (2)mac (3)windows");
Integer os = in.nextInt();
if (os <1 || os >3 ){
	System.out.println("Invalid OS selected");
return;
}
		Path resourcesDir = Paths.get("resources");

		String xorName;
switch (os){
	default:
		xorName = "Assembly-CSharp.xor";
break;
	case 1:
		xorName = "Assembly-CSharp-Linux.xor";
		break;
	case 2:
		xorName = "Assembly-CSharp-OSX.xor";
		break;
}

		Path outputFile = resourcesDir.resolve(xorName);
		Path origFile = managedDir.resolve(DLL_NAME + ".orig");
		Path patchedFile = managedDir.resolve(DLL_NAME);


		Files.deleteIfExists(outputFile);

		try (FileChannel outputChannel = FileChannel.open(outputFile, WRITE, CREATE_NEW);
		     FileChannel origChannel = FileChannel.open(origFile, READ);
		     FileChannel patchedChannel = FileChannel.open(patchedFile, READ)) {

			System.out.println("Unpatched size: " + origChannel.size() + "L");
			System.out.println("Patched size: " + patchedChannel.size() + "L");
			System.out.println("Unpatched SHA1: " + FileUtils.hashFile(origFile));
			System.out.println("Patched SHA1: " + FileUtils.hashFile(patchedFile));
			long maxSize = Math.max(origChannel.size(), patchedChannel.size());
			FileUtils.xorFiles(origChannel, patchedChannel, outputChannel, maxSize);
		}

		Path modLoaderPath = resourcesDir.resolve("ModLoader.dll");
		System.out.println("Mod Loader SHA1: " + FileUtils.hashFile(modLoaderPath));
	}
}
