import { AppStore } from "@/store";

enum WebSocketTriggers {
    CATEGORY_UPDATE = 'refresh-categories',
    PRODUCT_UPDATE = 'refresh-products',
    
} 

export class WebSocketService {

    private socket: WebSocket | null = null;
    private webSocketUrl: string = process.env.NEXT_PUBLIC_WS_URL!
    private appStore: AppStore;

    constructor(_store: AppStore){
        this.appStore = _store;
    }

    connect() {

        const { user } = this.appStore.getState();

        if (this.socket) {
            console.warn('WebSocket is already connected');
            return;
        }

        this.socket = new WebSocket(this.webSocketUrl + `?auth=${user.access_token}`);

        this.socket.onopen = () => {
            console.log('WebSocket connection established');
        };

        this.socket.onmessage = (event) => {
            console.log('Message received:', event.data);
        };

        this.socket.onerror = (error) => {
            console.error('WebSocket error:', error);
        };

        this.socket.onclose = () => {
            console.log('WebSocket connection closed');
            this.socket = null; // Reset the socket on close
        };

        this.setListeners()
    }

    send(message: string) {
        if (!this.socket || this.socket.readyState !== WebSocket.OPEN) {
            console.error('WebSocket is not open. Cannot send message:', message);
            return;
        }
        this.socket.send(message);
    }

    disconnect() {
        if (this.socket) {
            this.socket.close();
            this.socket = null;
        }
    }

    private setListeners() {

        this.socket && this.socket.addEventListener('message', (event) => {

            const wsTrigger: WebSocketTriggers = event.data as WebSocketTriggers

            switch (wsTrigger) {
                case WebSocketTriggers.CATEGORY_UPDATE:
                    window.dispatchEvent(new Event('update-categories'))
                    break
                case WebSocketTriggers.PRODUCT_UPDATE:
                    window.dispatchEvent(new Event('update-products'))
                    break
                default:
                    break
            }

            console.log('SystemUpdates message', event.data)
        
        })
    }
}


