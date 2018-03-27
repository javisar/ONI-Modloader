package oni.modloaderinstaller;


import java.nio.file.Path;

public enum OperatingSystem {

	WINDOWS {
		@Override
		public String getManagedPath() {
			return "OxygenNotIncluded_Data\\Managed";
		}

		@Override
		public String getExecutableName() {
			return "OxygenNotIncluded.exe";
		}

		@Override
		public String getExampleExecutablePath() {
			return "C:\\Program Files (x86)\\Steam\\steamapps\\common\\OxygenNotIncluded";
		}
	},

	MAC_OS {
		@Override
		public String getManagedPath() {
			return "OxygenNotIncluded.app/Contents/Resources/Data/Managed";
		}

		@Override
		public String getExecutableName() {
			return "OxygenNotIncluded.app";
		}

		@Override
		public String getExampleExecutablePath() {
			return "~/Library/Application Support/Steam/SteamApps/common/OxygenNotIncluded";
		}
	},

	LINUX {
		@Override
		public String getManagedPath() {
			return "OxygenNotIncluded_Data/Managed";
		}

		@Override
		public String getExecutableName() {
			return "OxygenNotIncluded.x86";
		}

		@Override
		public String getExampleExecutablePath() {
			return "~/.steam/steam/steamapps/common/OxygenNotIncluded";
		}

		@Override
		public String[] getExtensionFilter() {
			return new String[] {"OxygenNotIncluded.x86", "OxygenNotIncluded.x86_64"};
		}
	};

	public static final String MODS = "Mods";

	public static OperatingSystem getCurrent() {
		String os = System.getProperty("os.name").toLowerCase();
		if (os.startsWith("windows")) {
			return WINDOWS;
		} else if (os.startsWith("mac os") || os.startsWith("macos")) {
			return MAC_OS;
		} else {
			return LINUX;
		}
	}

	public abstract String getManagedPath();

	public abstract String getExecutableName();

	public abstract String getExampleExecutablePath();

	public String[] getExtensionFilter() {
		return new String[] {getExecutableName()};
	}

	public Path getDLLPath(Path executablePath) {
		return executablePath.resolveSibling(getManagedPath()).resolve(Constants.DLL_NAME);
	}

	public Path getModsDirectory(Path assemblyPath) {
		int levelsUp = (this == MAC_OS) ? 5 : 2;
		Path modsDir = assemblyPath;
		for (int i = 0; i < levelsUp; ++i) {
			modsDir = modsDir.getParent();
		}
		return modsDir.resolveSibling(MODS);
	}
}
