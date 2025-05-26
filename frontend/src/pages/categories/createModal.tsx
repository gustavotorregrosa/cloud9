import { Box, Button } from '@mui/material';
import Modal from '@mui/material/Modal';
import { Input } from '@mui/material';
import SaveAltIcon from '@mui/icons-material/SaveAlt';
import { useEffect, useState } from 'react';

interface ICreateCategoryModalProps {
    handleCreate: (name: string) => void
    setOpenCreateModalFn: (fn: () => void) => void
}

const CreateCategoryModal = ({setOpenCreateModalFn, handleCreate}: ICreateCategoryModalProps) => {

    const [name, setName] = useState<string>('')
    const [open, setOpen] = useState<boolean>(false)

    const openModal = () => {
        setOpen(true)
        setName('')
    }

    // useEffect(() => {
    //     setOpenCreateModalFn && setOpenCreateModalFn(openModal)
    // }, [])

    setOpenCreateModalFn && setOpenCreateModalFn(openModal)

    const _handleCreate = (_name: string) => {
        setOpen(false)
        handleCreate(_name)
    }

    const handleChange = (value: string) => {
        setName(value)
    }

    return <Modal
                open={open}
                onClose={() => setOpen(false)}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <p>Create Category</p>
                    <Input value={name} onChange={e => handleChange(e.target.value)} />&nbsp;&nbsp;
                    <Button onClick={() => _handleCreate(name)} variant="outlined" ><SaveAltIcon /></Button>
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
    // border: '2px solid #000',
    // boxShadow: 24,
    p: 4,
  };

export default CreateCategoryModal