import ResponsiveAppBar from "@/components/NavBar";
import { ConnectionServiceContext } from "@/context/ConnectionContext";
import { ConnectionService } from "@/services/connectionService";
import { WebSocketService } from "@/services/websocketService";
import { AppStore, makeStore } from "@/store";
import "@/styles/globals.css";
import type { AppProps } from "next/app";
import { useRef } from "react";
import { Provider } from 'react-redux'
import 'material-react-toastify/dist/ReactToastify.css';
import { ToastContainer } from "react-toastify";
import { SocketsContext } from "@/context/SocketsContext";
import {AlertModal} from '@/components/SendAlertModal';
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import { SendProductsModal } from "@/components/SendProductsModal";
// import { SystemUpdates } from "@/components/SystemUpdates";

export default function App({ Component, pageProps }: AppProps) {

  const storeRef = useRef<AppStore>()
  if(!storeRef.current){
    storeRef.current = makeStore()
  }

  const connectionServiceRef = useRef<ConnectionService>()
  if(!connectionServiceRef.current){
    connectionServiceRef.current = new ConnectionService(storeRef.current)
  }

  let openSendMessageModal: () => void

  let openSendEmailModal: () => void

  const websocketService = useRef<WebSocketService>()
  if (!websocketService.current) {
    websocketService.current = new WebSocketService(storeRef.current)
  }

  return <>
    <Provider store={storeRef.current}>
      <SocketsContext.Provider value={websocketService.current}>
        <ConnectionServiceContext.Provider value={connectionServiceRef.current}>
          <div className="mb-10"><ResponsiveAppBar openSendEmailModal={() => openSendEmailModal()} openSendMessageModal={() => openSendMessageModal()} /></div>
          <Component {...pageProps} />
          <ToastContainer />
          <SendProductsModal setOpenFn={fn => openSendEmailModal = fn} />
          <AlertModal setOpenFn={fn => openSendMessageModal = fn} />
          {/* <SystemUpdates /> */}
        </ConnectionServiceContext.Provider>
      </SocketsContext.Provider>
    </Provider>
  </>
}
