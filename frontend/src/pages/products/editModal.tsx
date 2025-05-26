import { Autocomplete, Box, Button, TextField } from '@mui/material';
import Modal from '@mui/material/Modal';
import { Input } from '@mui/material';
import SaveAltIcon from '@mui/icons-material/SaveAlt';
import { ICategory } from '../categories/index';
import { IProduct } from '.';
import { useState } from 'react';

interface IEditProductModalProps {
    categories: ICategory[]
    handleEdit: (product: IProduct) => void
    setOpenEditModalFn: (fn: (product: IProduct) => void) => void
}

const editProductModal = ({setOpenEditModalFn, handleEdit, categories}: IEditProductModalProps) => {

    const [product, setProduct] = useState<IProduct | null>()
    const [open, setOpen] = useState<boolean>(false)

    const openModal = (_product: IProduct) => {
        setOpen(true)
        setProduct(_product)
    }

    setOpenEditModalFn && setOpenEditModalFn(openModal)
    const _handleEdit = (product: IProduct) => {
        setOpen(false)
        handleEdit(product)
    }

    const handleChange = (value: string | ICategory , key: string) => {
        setProduct(prevProd => {
            
            if(!prevProd) {
                return null
            }

            return {
                ...prevProd,
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
                    <p>Edit Product</p>
                    <Autocomplete value={product?.category} onChange={(e, newValue) => handleChange(newValue!, 'category')} getOptionLabel={(category: ICategory) => category.name} getOptionKey={(category: ICategory) => category.id} options={categories} renderInput={params => <TextField {...params} label="Category" />} />
                        <br/>
                    <p>Name</p>
                    <Input value={product?.name} onChange={e => handleChange(e.target.value, 'name')} />&nbsp;&nbsp;
                    <Button onClick={() => _handleEdit(product as IProduct)} variant="outlined" ><SaveAltIcon /></Button>
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

export default editProductModal