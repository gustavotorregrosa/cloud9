package dev.torregrosa.cloud9;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;
import com.rabbitmq.client.DeliverCallback;

public class QueueListenerService {

    private EmailCustomService emailService;
    private AppConfig appConfig;

    public QueueListenerService(EmailCustomService emailService, AppConfig appConfig) {
        this.emailService = emailService;
        this.appConfig = appConfig;
    }

    public void listenToQueue() {
        String host = appConfig.getProperty("rabbitmq.host");
        String username = appConfig.getProperty("rabbitmq.username");
        String password = appConfig.getProperty("rabbitmq.password");
        int port = Integer.parseInt(appConfig.getProperty("rabbitmq.port"));
        String queueName = appConfig.getProperty("rabbitmq.queueName");
        String virtualHost = appConfig.getProperty("rabbitmq.virtualHost");

        ConnectionFactory factory = new ConnectionFactory();
        factory.setHost(host);
        factory.setUsername(username);
        factory.setPassword(password);
        factory.setPort(port);
        factory.setVirtualHost(virtualHost);

        try (Connection connection = factory.newConnection();
             Channel channel = connection.createChannel()) {

            channel.queueDeclare(queueName, true, false, false, null);
            System.out.println("Waiting for messages from queue: " + queueName);

            DeliverCallback deliverCallback = (consumerTag, delivery) -> {
                String message = new String(delivery.getBody(), java.nio.charset.StandardCharsets.UTF_8);

                System.out.println("Received message: " + message);

                try {
                    ObjectMapper objectMapper = new ObjectMapper();
                    ProductsMessage productsMessage = objectMapper.readValue(message, ProductsMessage.class);
                    System.out.println("Parsed ProductsMessage: " + productsMessage);

                    String bodyHTMLTable = """
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset="UTF-8">
                            <title>Product Information</title>
                            <style>
                                body {
                                    font-family: Arial, sans-serif;
                                    background-color: #f9f9f9;
                                    margin: 0;
                                    padding: 20px;
                                }
                                h3 {
                                    color: #333;
                                }
                                table {
                                    border-collapse: collapse;
                                    width: 100%;
                                    background-color: #fff;
                                    box-shadow: 0 2px 8px rgba(0,0,0,0.05);
                                }
                                th, td {
                                    border: 1px solid #ddd;
                                    padding: 12px 16px;
                                    text-align: left;
                                }
                                th {
                                    background-color: #f2f2f2;
                                    color: #333;
                                }
                                tr:nth-child(even) {
                                    background-color: #f9f9f9;
                                }
                            </style>
                        </head>
                        <body>
                            <h3>Product Information</h3>
                            <table>
                                <tr>
                                    <th>Product Name</th>
                                    <th>Product Category</th>
                                    <th>Product Description</th>
                                </tr>
                    """;
                    for (ProductsMessage.Product product : productsMessage.Products) {
                        bodyHTMLTable += String.format(
                            "<tr><td>%s</td><td>%s</td><td>%s</td></tr>",
                            product.Name, product.CategoryName, product.Description
                        );
                    }
                    bodyHTMLTable += """
                            </table>
                        </body>
                        </html>
                    """;

                 
                    this.emailService.sendEmail(
                            productsMessage.Email,
                            "Product Information",   
                            bodyHTMLTable
                    );
                
                } catch (Exception e) {
                    e.printStackTrace();    
                    System.out.println("Failed to process message: " + message);
                }

            };

            channel.basicConsume(queueName, true, deliverCallback, consumerTag -> { });

            while (true) {
                Thread.sleep(1000);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
    
}
