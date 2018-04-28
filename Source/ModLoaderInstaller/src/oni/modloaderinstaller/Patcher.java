package oni.modloaderinstaller;

import javafx.beans.property.StringProperty;
import javafx.concurrent.Task;

import java.io.IOException;
import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardCopyOption;

public class Patcher extends Task<Void> {

	public static Patcher patch(Path path, StringProperty messageProperty) {
		return patchOrUpdate(path, true, messageProperty);
	}

	public static Patcher update(Path path, StringProperty messageProperty) {
		return patchOrUpdate(path, false, messageProperty);
	}

	private static Patcher patchOrUpdate(Path path, boolean patch, StringProperty messageProperty) {
		Patcher patcher = new Patcher(path, patch);
		messageProperty.bind(patcher.messageProperty());
		Thread thread = new Thread(patcher);
		thread.start();
		return patcher;
	}

	private final Path path;
	private final boolean patchAssembly;

	private Patcher(Path pathToAssembly, boolean patchAssembly) {
		this.path = pathToAssembly;
		this.patchAssembly = patchAssembly;
	}

	@Override
	protected Void call() throws Exception {
		if (patchAssembly) {
			updateMessage("Patching assembly...");

			try {
				FileUtils.applyXOR(path, Constants.PATCHED_SIZE);


			} catch (Throwable t) {
				updateMessage("Patching failed. The original file has been restored.");
				throw t;
			}
		}

		updateMessage("Copying libraries...");

		try {
			installLibraries();
		} catch (Throwable t) {
			updateMessage("The game was successfully patched, but copying libraries failed.");
			throw t;
		}

		updateMessage("Done!");
		return null;
	}

	private void installLibraries() throws IOException {
		Path modLoaderFile = path.resolveSibling("ModLoader.dll");
		copyResource("/ModLoader.dll", modLoaderFile);

		Path harmonyFile = path.resolveSibling("0Harmony.dll");
		copyResource("/0Harmony.dll", harmonyFile);

		Path modsDir = OperatingSystem.getCurrent().getModsDirectory(path);
		Files.createDirectories(modsDir);

	}

	private static void copyResource(String resource, Path targetPath) throws IOException {
		try (InputStream inputStream = Patcher.class.getResourceAsStream(resource)) {
			Files.copy(inputStream, targetPath, StandardCopyOption.REPLACE_EXISTING);
		}
	}
}
