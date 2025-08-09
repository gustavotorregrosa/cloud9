package dev.torregrosa.cloud9;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class AppConfig {
    private Properties properties;

    public AppConfig() {
        properties = new Properties();
        loadProperties();
    }

    private void loadProperties() {
        try (InputStream input = getClass().getClassLoader().getResourceAsStream("application.properties")) {
            if (input == null) {
                System.out.println("Sorry, unable to find application.properties");
                return;
            }
            properties.load(input);
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    public String getProperty(String key) {
        return properties.getProperty(key);
    }
}
