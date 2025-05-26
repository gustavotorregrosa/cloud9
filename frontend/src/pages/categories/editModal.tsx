import { Box, Button } from '@mui/material';
import Modal from '@mui/material/Modal';
import { Input } from '@mui/material';
import SaveAltIcon from '@mui/icons-material/SaveAlt';
import { ICategory } from '.';
import { useState } from 'react';

interface IEditCategoryModalProps {
    handleEdit: (category: ICategory) => void
    setOpenEditModalFn: (fn: (category: ICategory) => void) => void
}

const editCategoryModal = ({setOpenEditModalFn, handleEdit}: IEditCategoryModalProps) => {

    const [category, setCategory] = useState<ICategory | null>()
    const [open, setOpen] = useState<boolean>(false)

    const openModal = (_category: ICategory) => {
        setOpen(true)
        setCategory(_category)
    }

    setOpenEditModalFn && setOpenEditModalFn(openModal)
    const _handleEdit = (category: ICategory) => {
        setOpen(false)
        handleEdit(category)
    }

    const handleChange = (value: string | number, key: string) => {
        setCategory(prevCat => {
            if(!prevCat) {
                return null
            }

            return {
                ...prevCat,
                [key]: value
            }
        })
    }

    return <Modal
                open={open}
                onClose={() => setOpen(false)}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <p>Edit Category</p>
                    <Input value={category?.name} onChange={e => handleChange(e.target.value, 'name')} />&nbsp;&nbsp;
                    <Button onClick={() => _handleEdit(category as ICategory)} variant="outlined" ><SaveAltIcon /></Button>
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

export default editCategoryModal