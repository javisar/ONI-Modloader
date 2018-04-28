package oni.modloaderinstaller;


public final class Constants {

	private Constants() {
		throw new Error("No instances!");
	}

	public static final String ONI_VERSION = "259080";
	public static final String DLL_NAME = "Assembly-CSharp.dll";
	public static final String XOR_NAME;

	static {
		switch  (OperatingSystem.getCurrent()) {
            default:
                XOR_NAME = "Assembly-CSharp.xor";
                UNPATCHED_SIZE = 4566528L;
                PATCHED_SIZE = 4582912L;
                UNPATCHED_SHA1 = "a578c0b248804b24fcf7566183a0283f839f9945";
                PATCHED_SHA1 = "c02fcf2e771a787f56093cc83af7382c9128c62";
                break;
            case MAC_OS:
                XOR_NAME = "Assembly-CSharp-OSX.xor";
                UNPATCHED_SIZE = 4566528L;
                PATCHED_SIZE = 4582400L;
                UNPATCHED_SHA1 = "12646b6968e502df6c560ae2525532ee8cb121b3";
                PATCHED_SHA1 = "aa9d10ca5a8a021fbb30e96bca10e19a79d1490b";
                break;
           // case LINUX:
             //   XOR_NAME = "Assembly-CSharp-Linux.xor";
               // UNPATCHED_SIZE = 4566528L;
               // PATCHED_SIZE = 4582400L;
                //UNPATCHED_SHA1 = "12646b6968e502df6c560ae2525532ee8cb121b3";
                //PATCHED_SHA1 = "856c0bd9a5e83160b766f71325befe37aa623f2";    break;
        }



	}

	public static final long UNPATCHED_SIZE;

	public static final long PATCHED_SIZE;
	public static final String UNPATCHED_SHA1;
	public static final String PATCHED_SHA1;

	public static final String MOD_LOADER_SHA1 = "2b31e9ceb956b7c627aaf17be04f9128b71e3df7";
	public static final String ERROR_SHA1 = "- ERROR -";


}
