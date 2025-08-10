package dev.torregrosa.cloud9;

public class App {
    
    public static void main(String[] args) {

        AppConfig config = new AppConfig();
        EmailCustomService emailService = new EmailCustomService(config);
        QueueListenerService queueListenerService = new QueueListenerService(emailService, config);

        queueListenerService.listenToQueue();

    }
}