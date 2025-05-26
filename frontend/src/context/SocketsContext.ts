import { createContext } from "react";
import { WebSocketService } from "@/services/websocketService";

export const SocketsContext = createContext<WebSocketService | null>(null)

