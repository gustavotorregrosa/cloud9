import { createContext } from "react";

export interface ISocketsGroup {
    // category: Socket
    // product: Socket
    // message: Socket,
    systemUpdates: WebSocket
}

export const SocketsContext = createContext<ISocketsGroup | null>(null)

