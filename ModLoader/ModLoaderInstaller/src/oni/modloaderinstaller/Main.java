package oni.modloaderinstaller;


import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.image.Image;
import javafx.stage.Stage;

public class Main extends Application {

	@Override
	public void start(Stage primaryStage) throws Exception {
		Parent root = FXMLLoader.load(getClass().getResource("/oni/modloaderinstaller/MainPanel.fxml"));
		Scene scene = new Scene(root);
		primaryStage.setScene(scene);

		primaryStage.getIcons().add(new Image(getClass().getResourceAsStream("/icon.png")));
		primaryStage.sizeToScene();
		primaryStage.setResizable(false);
		primaryStage.setTitle("ONI Mod Loader Installer");
		primaryStage.show();
	}
}
