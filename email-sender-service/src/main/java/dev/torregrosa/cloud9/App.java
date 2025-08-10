package dev.torregrosa.cloud9;

public class App {
    
    public static void main(String[] args) {

        AppConfig config = new AppConfig();
        EmailService emailService = new EmailService(config);
        QueueListenerService queueListenerService = new QueueListenerService(emailService, config);

        queueListenerService.listenToQueue();

    }
}