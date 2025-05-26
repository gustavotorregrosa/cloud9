import { Box, Button } from '@mui/material';
import Modal from '@mui/material/Modal';;
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import { IProduct } from '.';
import { useState } from 'react';

interface IDeleteProductModalProps {
    handleDelete: (product: IProduct) => void
    setOpenDeleteModalFn: (fn: (product: IProduct) => void) => void
}

const deleteProductModal = ({setOpenDeleteModalFn, handleDelete}: IDeleteProductModalProps) => {

    const [product, setProduct] = useState<IProduct | null>()
    const [open, setOpen] = useState<boolean>(false)

    const openModal = (_product: IProduct) => {
        setOpen(true)
        setProduct(_product)
    }

    setOpenDeleteModalFn && setOpenDeleteModalFn(openModal)

    const _handlDelete = (product: IProduct) => {
        setOpen(false)
        handleDelete(product)
    }


    return <Modal
                open={open}
                onClose={() => setOpen(false)}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <p>Delete the product {product?.name}?&nbsp;&nbsp;&nbsp;&nbsp;<Button onClick={() => _handlDelete(product as IProduct)} variant="outlined"><DeleteOutlineIcon /></Button></p>
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

export default deleteProductModal