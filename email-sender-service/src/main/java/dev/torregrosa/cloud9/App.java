package dev.torregrosa.cloud9;

public class App {
    
    public static void main(String[] args) {

        AppConfig config = new AppConfig();
        EmailService emailService = new EmailService(config);
        QueueListenerService queueListenerService = new QueueListenerService(emailService, config);

        // Start the queue listener in a separate thread
        new Thread(queueListenerService::listenToQueue).start();
    }
}