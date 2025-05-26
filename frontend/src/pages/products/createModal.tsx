import { Autocomplete, Box, Button, TextField } from '@mui/material';
import Modal from '@mui/material/Modal';
import { Input } from '@mui/material';
import SaveAltIcon from '@mui/icons-material/SaveAlt';
import { useState } from 'react';
import { ICategory } from '../categories';
import { IProduct } from '.';

interface ICreateProductModalProps {
    handleCreate: (product: IProduct) => void
    setOpenCreateModalFn: (fn: () => void) => void
    categories: ICategory[]
}

const CreateProductModal = ({setOpenCreateModalFn, handleCreate, categories}: ICreateProductModalProps) => {

    const [name, setName] = useState<string>('')
    const [open, setOpen] = useState<boolean>(false)
    const initialProd: IProduct = {
        name: '',
        description: '',
        category: {
            id: '',
            name: ''
        }
    }

    const [product, setProduct] = useState<IProduct>(initialProd)

    const openModal = () => {
        setOpen(true)
        setName('')
        setProduct(initialProd)
    }

    setOpenCreateModalFn && setOpenCreateModalFn(openModal)

    const _handleCreate = (product: IProduct) => {
        setOpen(false)
        handleCreate(product)
        
    }

    const handleChange = (value: string | ICategory , key: string) => {
        setProduct(prevProd => {

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
                    <p>Create Product</p>
                    <Autocomplete value={product?.category} onChange={(e, newValue) => handleChange(newValue!, 'category')} getOptionLabel={(category: ICategory) => category.name} getOptionKey={(category: ICategory) => category.id} options={categories} renderInput={params => <TextField {...params} label="Category" />} />
                    <br/>
                    
                    <Input value={product?.name} onChange={e => {
                        handleChange(e.target.value, 'name')
                        console.log({e})
                    }} />&nbsp;&nbsp;
                    <Button onClick={() => _handleCreate(product)} variant="outlined" ><SaveAltIcon /></Button>
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

export default CreateProductModal