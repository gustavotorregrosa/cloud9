import ResponsiveAppBar from "@/components/NavBar";
import { ConnectionServiceContext } from "@/context/ConnectionContext";
import { ConnectionService } from "@/services/connectionService";
import { AppStore, makeStore } from "@/store";
import "@/styles/globals.css";
import type { AppProps } from "next/app";
import { useEffect, useRef } from "react";
import { Provider } from 'react-redux'
import 'material-react-toastify/dist/ReactToastify.css';
import { ToastContainer } from "react-toastify";
import io, { Socket } from 'socket.io-client'
import { SocketsContext } from "@/context/SocketsContext";
import {AlertModal} from '@/components/SendAlertModal';

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import { SystemUpdates } from "@/components/SystemUpdates";
import { IUser } from "@/store/user/user.interface";
import { login } from "@/store/user/user.slice";

export default function App({ Component, pageProps }: AppProps) {

  const storeRef = useRef<AppStore>()
  if(!storeRef.current){
    storeRef.current = makeStore()
  }

  const connectionServiceRef = useRef<ConnectionService>()
  if(!connectionServiceRef.current){
    connectionServiceRef.current = new ConnectionService(storeRef.current)
  }

  const systemUpdatesSocket = useRef<WebSocket>()

  const sockets = {
    // category: categorySocket.current,
    // product: productSocket.current,
    // message: messageSocket.current,
    systemUpdates: systemUpdatesSocket.current
  }

  let openSendMessageModal: () => void

  return <>
    <Provider store={storeRef.current}>
      <SocketsContext.Provider value={sockets}>
        <ConnectionServiceContext.Provider value={connectionServiceRef.current}>
          <div className="mb-10"><ResponsiveAppBar openSendMessageModal={() => openSendMessageModal()} /></div>
          <Component {...pageProps} />
          <ToastContainer />
          <AlertModal setOpenFn={fn => openSendMessageModal = fn} />
          <SystemUpdates />
        </ConnectionServiceContext.Provider>
      </SocketsContext.Provider>
    </Provider>
  </>
}
