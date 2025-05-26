import { SocketsContext } from '@/context/SocketsContext';
import { Box, Button, Input } from '@mui/material';
import Modal from '@mui/material/Modal';
import { useContext, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

interface AlertModalProps {
    setOpenFn: (fn: () => void) => void
}

export const AlertModal = ({setOpenFn}: AlertModalProps) => {
    const sockets = useContext(SocketsContext)

    const [message, setMessage] = useState<string>()
    const [open, setOpen] =  useState<boolean>(false)

    const openModal = () => {
        setMessage(undefined)
        setOpen(true)
    }

    setOpenFn && setOpenFn(openModal)

    // useEffect(() => {
    //     sockets && sockets.message.connect()
    //     const mappedMessageEvent = sockets?.message.on('show-message', message => {
    //     toast.success(message, {
    //         toastId: message
    //     })
    //     })
    
    //       return () => {
    //             sockets && sockets.message.disconnect()
    //             mappedMessageEvent?.close()
    //         }
    //   }, [])
    
    // const sendMessage = () => {
    //     setOpen(false)
    //     sockets && sockets.message.emit('send-message', {message})
    // }

    const sendMessage = () => {
        console.log({message})
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

