package dev.torregrosa.cloud9;

public class App {

    public static void main(String[] args) {

        AppConfig config = new AppConfig();
        String email = config.getProperty("email.username");
        String password = config.getProperty("email.app.password");

        System.out.println("Email: " + email);
        System.out.println("Password: " + password);

        // QueueListenerService queueListenerService = new QueueListenerService();
        // queueListenerService.listenToQueue();
    }
}