package oni.modloaderinstaller;

import javafx.scene.paint.Paint;

import static oni.modloaderinstaller.Constants.*;

public enum FileStatus {
	NO_FILE("", "Patch", Paint.valueOf("BLACK")),
	VALID_UNPATCHED("Valid, unpatched", "Patch", Paint.valueOf("GREEN")),
	VALID_OUTDATED("Valid, outdated", "Update", Paint.valueOf("DARKORANGE")),
	VALID_PATCHED("Valid, patched", "Unpatch", Paint.valueOf("GREEN")),
	INVALID("Invalid", "Patch", Paint.valueOf("RED")),
	ERROR("ERROR", "Patch", Paint.valueOf("RED"));

	private final String displayName;
	private final String buttonText;
	private final Paint displayColor;

	FileStatus(String displayName, String buttonText, Paint displayColor) {
		this.displayName = displayName;
		this.buttonText = buttonText;
		this.displayColor = displayColor;
	}

	public String getDisplayName() {
		return displayName;
	}

	public String getButtonText() {
		return buttonText;
	}

	public Paint getDisplayColor() {
		return displayColor;
	}

	public boolean isValid() {
		return (this == VALID_UNPATCHED || this == VALID_OUTDATED || this == VALID_PATCHED);
	}

	public static FileStatus forHashes(String dllHash, String modLoaderHash) {
		if (dllHash == null || dllHash.isEmpty()) return NO_FILE;

        if (Constants.UNPATCHED_SHA1.equals(dllHash)) return VALID_UNPATCHED;
        if (PATCHED_SHA1.equals(dllHash)) {
            if (MOD_LOADER_SHA1.equals(modLoaderHash))
                return VALID_PATCHED;
            return VALID_OUTDATED;
        }
        if (ERROR_SHA1.equals(dllHash)) return ERROR;
        return INVALID;
    }


}
