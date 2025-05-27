import { SocketsContext } from '@/context/SocketsContext';
import { Box, Button, Input } from '@mui/material';
import Modal from '@mui/material/Modal';
import { useContext, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

interface AlertModalProps {
    setOpenFn: (fn: () => void) => void
}

export const AlertModal = ({setOpenFn}: AlertModalProps) => {
    const socketService = useContext(SocketsContext)

    const [message, setMessage] = useState<string>()
    const [open, setOpen] =  useState<boolean>(false)

    const openModal = () => {
        setMessage(undefined)
        setOpen(true)
    }

    const listenForMessages = () => {
        window.addEventListener('pop-up-message', (event) => {
            const detail = (event as CustomEvent).detail
            if (detail?.message) {
                toast(detail.message, {
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                });
            }
            
        })
    }

    useEffect(() => {
        listenForMessages()
    }, [])

    setOpenFn && setOpenFn(openModal)

    const sendMessage = () => {
        socketService?.send(message || '')
        setOpen(false)
    }
    
    return <Modal open={open} onClose={() => setOpen(false)}>
        <Box sx={style}>
            <Input value={message} onChange={e => { setMessage(e.target.value) }} />&nbsp;&nbsp;
            <br/><br/>
            <Button onClick={() => sendMessage()} variant="outlined" >Broadcast message</Button>
        </Box>
    </Modal>
}

const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    maxWidth: 400,
    bgcolor: 'background.paper',
    p: 4,
  };

