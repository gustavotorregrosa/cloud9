import { ConnectionService } from "@/services/connectionService";
import { createContext } from "react";

export const ConnectionServiceContext = createContext<ConnectionService | null>(null)