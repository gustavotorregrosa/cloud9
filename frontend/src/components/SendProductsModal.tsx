import { ConnectionServiceContext } from "@/context/ConnectionContext";
import { useContext, useState } from "react";
import { Box, Button, Input } from '@mui/material';
import Modal from '@mui/material/Modal';

interface SendProductsModalProps {
    setOpenFn: (fn: () => void) => void
}

export const SendProductsModal = ({ setOpenFn }: SendProductsModalProps) => {
    
    const connectionService = useContext(ConnectionServiceContext)
    
    const [email, setEmail] = useState<string>()
    const [open, setOpen] =  useState<boolean>(false)

    const openModal = () => {
        setEmail(undefined)
        setOpen(true)
    }

    setOpenFn && setOpenFn(openModal)


    const sendProductsToEmail = () => {
        connectionService?.makeRequest<void>('products/product-list', 'post', JSON.stringify({email}))
        setOpen(false)
    }



      return <Modal open={open} onClose={() => setOpen(false)}>
        <Box sx={style}>
            <Input value={email} onChange={e => { setEmail(e.target.value) }} />&nbsp;&nbsp;
            <br/><br/>
            <Button onClick={() => sendProductsToEmail()} variant="outlined" >Send product list</Button>
        </Box>
    </Modal>
};

const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    maxWidth: 400,
    bgcolor: 'background.paper',
    p: 4,
  };

