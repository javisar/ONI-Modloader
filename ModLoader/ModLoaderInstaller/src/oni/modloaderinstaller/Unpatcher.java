package oni.modloaderinstaller;

import javafx.beans.property.StringProperty;
import javafx.concurrent.Task;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;

public class Unpatcher extends Task<Void> {

	public static Unpatcher unpatch(Path path, StringProperty messageProperty) {
		Unpatcher unpatcher = new Unpatcher(path);
		messageProperty.bind(unpatcher.messageProperty());
		Thread thread = new Thread(unpatcher);
		thread.start();
		return unpatcher;
	}

	private final Path path;

	private Unpatcher(Path pathToFile) {
		this.path = pathToFile;
	}

	@Override
	protected Void call() throws Exception {
		updateMessage("Unpatching assembly...");

		try {
            FileUtils.applyXOR(path, Constants.UNPATCHED_SIZE);


		} catch (Throwable t) {
			updateMessage("Unpatching failed. The modded file has been restored.");
			throw t;
		}

		updateMessage("Removing libraries...");

		try {
			uninstallLibraries();
		} catch (Throwable t) {
			updateMessage("The game files were successfully restored, but deleting libraries failed.");
			t.printStackTrace();
			return null; // Ignore exception in this case
		}

		updateMessage("Done!");
		return null;
	}

	private void uninstallLibraries() throws IOException {
		Path modLoaderFile = path.resolveSibling("ModLoader.dll");
		Files.deleteIfExists(modLoaderFile);

		Path harmonyFile = path.resolveSibling("0Harmony.dll");
		Files.deleteIfExists(harmonyFile);

		Path rewiredBackup = path.resolveSibling("Rewired_Core.dll.bak");
		Files.deleteIfExists(rewiredBackup);
	}
}
