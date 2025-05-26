import { Box, Button } from '@mui/material';
import Modal from '@mui/material/Modal';;
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import { ICategory } from '.';
import { useEffect, useState } from 'react';

interface IDeleteCategoryModalProps {
    handleDelete: (category: ICategory) => void
    setOpenDeleteModalFn: (fn: (category: ICategory) => void) => void
}

const deleteCategoryModal = ({setOpenDeleteModalFn, handleDelete}: IDeleteCategoryModalProps) => {

    const [category, setCategory] = useState<ICategory | null>()
    const [open, setOpen] = useState<boolean>(false)

    const openModal = (_category: ICategory) => {
        setOpen(true)
        setCategory(_category)
    }

    setOpenDeleteModalFn && setOpenDeleteModalFn(openModal)

    const _handlDelete = (category: ICategory) => {
        setOpen(false)
        handleDelete(category)
    }

    return <Modal
                open={open}
                onClose={() => setOpen(false)}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <p>Delete the category {category?.name}?&nbsp;&nbsp;&nbsp;&nbsp;<Button onClick={() => _handlDelete(category as ICategory)} variant="outlined" ><DeleteOutlineIcon /></Button></p>
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

export default deleteCategoryModal