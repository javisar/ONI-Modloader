package oni.modloaderinstaller;

import java.nio.channels.FileChannel;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Scanner;

import static java.nio.file.StandardOpenOption.*;
import static oni.modloaderinstaller.Constants.DLL_NAME;

public class GetNewModLoaderHash {

	public static void main(String[] args) throws Exception {
		System.out.println("Verify that the original DLL is called");
		System.out.println("  " + DLL_NAME + ".orig");

		Path resourcesDir = Paths.get("resources");

Path modLoaderPath = resourcesDir.resolve("ModLoader.dll");
		System.out.println("Mod Loader SHA1: " + FileUtils.hashFile(modLoaderPath));
	}
}
