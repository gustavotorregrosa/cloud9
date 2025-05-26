import { SocketsContext } from '@/context/SocketsContext';
import { IState } from '@/store';
import { useContext, useEffect } from 'react';
import { useSelector } from 'react-redux';


enum WebSocketTriggers {
    CATEGORY_UPDATE = 'refresh-categories',
    PRODUCT_UPDATE = 'refresh-products',
    
} 

export const SystemUpdates = () => {
    const sockets = useContext(SocketsContext)
    const user = useSelector((state: IState) => state.user)

    const initSocket = () => {
        const webSocketUrl: string = process.env.NEXT_PUBLIC_WS_URL!
    
        if(sockets?.systemUpdates){
            return
        }

        sockets!.systemUpdates = new WebSocket(webSocketUrl + `?auth=${user.access_token}`)

        sockets!.systemUpdates.onerror = (event) => {
            console.error('SystemUpdates error', event)
        }

        sockets!.systemUpdates.onopen = () => {
            console.log('SystemUpdates connected', {sockets})

             setInterval(() => {
                sockets!.systemUpdates.send(JSON.stringify({
                    type: 'ping',
                    data: {
                        user: user.email
                    }
                }))
            

            }, 4000)


        }

        sockets!.systemUpdates.addEventListener('message', (event) => {

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

    const closeSocket = () => {
        sockets?.systemUpdates && sockets.systemUpdates.close()
    }

    useEffect(() => {

        if(!user.email){
            return
        }

        initSocket()
        return () => closeSocket()
    }, [user])

    return <></>
}


