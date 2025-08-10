package dev.torregrosa.cloud9;

import java.util.Properties;

import jakarta.mail.Authenticator;
import jakarta.mail.Message;
import jakarta.mail.MessagingException;
import jakarta.mail.PasswordAuthentication;
import jakarta.mail.Session;
import jakarta.mail.Transport;
import jakarta.mail.internet.InternetAddress;
import jakarta.mail.internet.MimeMessage;

public class EmailService {
    
    private AppConfig config;

    public EmailService(AppConfig config) {
        this.config = config;
    }

    public void sendEmail(String to, String subject, String body) {
        Properties props = new Properties();
        // props.put("mail.smtp.auth", this.config.getProperty("mail.smtp.auth"));
        // props.put("mail.smtp.starttls.enable", this.config.getProperty("mail.smtp.starttls.enable"));
        // props.put("mail.smtp.host", this.config.getProperty("mail.smtp.host"));
        // props.put("mail.smtp.port", this.config.getProperty("mail.smtp.port"));

        props.put("mail.smtp.auth", "true");
        props.put("mail.smtp.starttls.enable", "true");
        props.put("mail.smtp.host", "smtp.gmail.com");
        props.put("mail.smtp.port", "587");

        Session session = Session.getInstance(props, new Authenticator() {
            @Override
            protected PasswordAuthentication getPasswordAuthentication() {
                String username = config.getProperty("email.username");
                String appPassword = config.getProperty("email.app.password");
                System.out.println("Email username: " + username);
                System.out.println("Email app password: " + appPassword);

                return new PasswordAuthentication(username, appPassword);
            }
        });

        try {
            Message message = new MimeMessage(session);
            message.setFrom(new InternetAddress(to));
            message.setRecipients(Message.RecipientType.TO, InternetAddress.parse(to));
            message.setSubject(subject);
            message.setContent(body, "text/html");
            Transport.send(message);
            System.out.println("Email sent successfully to " + to);
        } catch (MessagingException e) {
            e.printStackTrace();
        }
    }


}
