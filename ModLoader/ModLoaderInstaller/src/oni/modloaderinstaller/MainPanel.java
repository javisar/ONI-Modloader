package oni.modloaderinstaller;

import javafx.application.Platform;
import javafx.beans.binding.Bindings;
import javafx.beans.binding.ObjectBinding;
import javafx.beans.property.*;
import javafx.concurrent.Task;
import javafx.fxml.FXML;
import javafx.geometry.Insets;
import javafx.scene.control.*;
import javafx.scene.control.Alert.AlertType;
import javafx.scene.image.Image;
import javafx.scene.layout.GridPane;
import javafx.scene.layout.Priority;
import javafx.stage.FileChooser;
import javafx.stage.FileChooser.ExtensionFilter;
import javafx.stage.Stage;
import javafx.stage.Window;

import java.io.File;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.nio.file.Files;
import java.nio.file.InvalidPathException;
import java.nio.file.Path;
import java.nio.file.Paths;

import static oni.modloaderinstaller.Constants.ONI_VERSION;
import static oni.modloaderinstaller.Constants.DLL_NAME;

public class MainPanel {

	private static final OperatingSystem OPERATING_SYSTEM = OperatingSystem.getCurrent();

	public final Property<Path> selectedFile = new SimpleObjectProperty<>(null);
	public final Property<FileStatus> fileStatusProperty = new SimpleObjectProperty<>(FileStatus.NO_FILE);
	public final ObjectBinding<FileStatus> fileStatusBinding = Bindings.createObjectBinding(this::calculateHashes, selectedFile);
	public final BooleanProperty working = new SimpleBooleanProperty(false);

	public TextField exePathTextField;
	public TextField dllPathTextField;

	public Button fileSelectButton;
	public Button patchButton;

	public Label instructionsLabel;
	public Label fileSelectLabel;
	public Label fileStatusLabel;
	public Label progressStatusLabel;

	@FXML
	private void initialize() {
		// Replace constants in instructions label
		instructionsLabel.setText(instructionsLabel.getText()
				.replace("${version}", ONI_VERSION)
				.replace("${exe_path}", OPERATING_SYSTEM.getExampleExecutablePath())
				.replace("${dll_name}", DLL_NAME));
		// ... and in the tld executable selection label
		fileSelectLabel.setText(fileSelectLabel.getText()
				.replace("${exe_name}", OPERATING_SYSTEM.getExecutableName()));

		// Selected file depends on the path in the text field
		// Not bound the "traditional" way as that caused duplicate calls to createPathFromText
		dllPathTextField.textProperty().addListener((o) -> createPathFromText());

		// Only able to change file path when not currently patching
		dllPathTextField.disableProperty().bind(working);

		// File status depends on selected file
		fileStatusProperty.bind(fileStatusBinding);

		// File status label responds to selected file status
		fileStatusLabel.textProperty().bind(Bindings.createStringBinding(
				() -> fileStatusProperty.getValue().getDisplayName(), fileStatusProperty));
		fileStatusLabel.textFillProperty().bind(Bindings.createObjectBinding(
				() -> fileStatusProperty.getValue().getDisplayColor(), fileStatusProperty));

		// File select button should be focused (runLater -> when Scene graph is established)
		Platform.runLater(() -> fileSelectButton.requestFocus());

		// Patch button should only be enabled if file is valid and not currently patching
		patchButton.disableProperty().bind(
				Bindings.createBooleanBinding(() -> !fileStatusProperty.getValue().isValid(), fileStatusProperty)
						.or(working));
		patchButton.textProperty().bind(Bindings.createStringBinding(
				() -> fileStatusProperty.getValue().getButtonText(), fileStatusProperty));
	}

	private FileStatus calculateHashes() {
		Path path = selectedFile.getValue();
		if (path == null) return FileStatus.NO_FILE;

		String dllHash = FileUtils.hashFile(path);
		String modLoaderHash = FileUtils.hashFile(path.resolveSibling("ModLoader.dll"));
		return FileStatus.forHashes(dllHash, modLoaderHash);


	}

	private void createPathFromText() {
		if (!dllPathTextField.isFocused()) return;

		String text = dllPathTextField.getText();
		progressStatusLabel.setText("");
		exePathTextField.setText("");
		selectedFile.setValue(null);

		if (text.startsWith("\"") && text.endsWith("\"")) text = text.substring(1, text.length() - 1);
		if (!text.endsWith(DLL_NAME)) return;

		try {
			Path path = Paths.get(text);
			if (!Files.isRegularFile(path)) {
				progressStatusLabel.setText("Could not find DLL file");
				return;
			} else if (!Files.isReadable(path) || !Files.isWritable(path)) {
				error("Read error", "The DLL file you selected cannot be read from or written to.\n\n" +
						"Make sure that the file is not marked read-only and\n" +
						"that you have the required permissions to write to it.");
				return;
			}

			selectedFile.setValue(path);
		} catch (InvalidPathException ipe) {
			progressStatusLabel.setText("Invalid path");
		}
	}

	public void selectFile() {
		FileChooser fileChooser = new FileChooser();
		fileChooser.setTitle("Select \"" + OPERATING_SYSTEM.getExecutableName() + "\"");
		ExtensionFilter exeFilter = new ExtensionFilter("ONI Executable", OPERATING_SYSTEM.getExtensionFilter());
		fileChooser.getExtensionFilters().add(exeFilter);
		ExtensionFilter dllFilter = new ExtensionFilter(DLL_NAME, DLL_NAME);
		fileChooser.getExtensionFilters().add(dllFilter);
		fileChooser.setSelectedExtensionFilter(exeFilter);

		Window window = fileSelectButton.getScene().getWindow();
		File selected = fileChooser.showOpenDialog(window);
		if (selected == null || !selected.exists()) return;

		Path executablePath;
		Path dllPath;
		if (DLL_NAME.equals(selected.getName())) {
			executablePath = null;
			dllPath = selected.toPath();
		} else {
			executablePath = selected.toPath();
			dllPath = OPERATING_SYSTEM.getDLLPath(executablePath);
		}

		progressStatusLabel.setText("");
		exePathTextField.setText(executablePath != null ? executablePath.toString() : "");
		dllPathTextField.setText(dllPath.toString());

		if (Files.isRegularFile(dllPath)) {
			selectedFile.setValue(dllPath);
			patchButton.requestFocus();
		} else {
			selectedFile.setValue(null);
			error("File error", "Expected the file \"" + DLL_NAME + "\" at path \n\"" + dllPath + "\",\n" +
					"but no such file was present.");
		}
	}

	public void patchOrUnpatch() {
		if (working.get()) return;
		Path path = selectedFile.getValue();
		if (path == null || !Files.isRegularFile(path) || !Files.isWritable(path)) {
			error("Patch error", "The specified file was invalid or was not writable.");
			return;
		}

		FileStatus fileStatus = fileStatusProperty.getValue();
		StringProperty messageProperty = progressStatusLabel.textProperty();

		Task<Void> task;
		switch (fileStatus) {
			case VALID_UNPATCHED:
				task = Patcher.patch(path, messageProperty);
				break;
			case VALID_OUTDATED:
				task = Patcher.update(path, messageProperty);
				break;
			case VALID_PATCHED:
				task = Unpatcher.unpatch(path, messageProperty);
				break;
			default:
				return;
		}

		working.set(true);

		task.setOnSucceeded(ignoredArg -> {
			fileStatusBinding.invalidate();
			working.set(false);
			patchButton.requestFocus();
		});
		task.setOnFailed(ignoredArg -> {
			error("Patching failed", "Patching error", messageProperty.get(), task.getException());
		});
	}

	private static void error(String title, String text) {
		Alert alert = new Alert(AlertType.ERROR);
		setIcon(alert);
		alert.setTitle(title);
		alert.setContentText(text);

		alert.showAndWait();
	}

	private static void error(String title, String header, String text, Throwable cause) {
		StringWriter stringWriter = new StringWriter(); // Doesn't need to be closed & is unbuffered
		try (PrintWriter writer = new PrintWriter(stringWriter)) {
			cause.printStackTrace(writer);
		}

		Alert alert = new Alert(AlertType.ERROR);
		setIcon(alert);
		alert.setTitle(title);
		alert.setHeaderText(header);

		String actualText = text + "\n\nPlease append this exception stacktrace when reporting this error:";
		Label textLabel = new Label(actualText);
		textLabel.setPadding(new Insets(0, 0, 10, 0));

		TextArea exceptionText = new TextArea(stringWriter.toString());
		exceptionText.setEditable(false);
		exceptionText.setMaxHeight(Double.MAX_VALUE);
		exceptionText.setMaxWidth(Double.MAX_VALUE);
		GridPane.setHgrow(exceptionText, Priority.ALWAYS);
		GridPane.setVgrow(exceptionText, Priority.ALWAYS);

		GridPane alertContent = new GridPane();
		alertContent.setMaxWidth(Double.MAX_VALUE);
		alertContent.add(textLabel, 0, 0);
		alertContent.add(exceptionText, 0, 1);

		alert.getDialogPane().setContent(alertContent);
		alert.showAndWait();
	}

	private static void setIcon(Dialog<?> dialog) {
		Window window = dialog.getDialogPane().getScene().getWindow();
		if (window instanceof Stage) {
			Stage stage = (Stage) window;
			stage.getIcons().add(new Image(Main.class.getResourceAsStream("/icon.png")));
		}
	}
}
