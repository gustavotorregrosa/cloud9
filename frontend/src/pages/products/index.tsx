import { useContext, useEffect, useState } from "react"
import { ICategory } from "../categories"
import { ConnectionServiceContext } from "@/context/ConnectionContext"
import { Button, Container } from "@mui/material"
import AddIcon from '@mui/icons-material/Add';
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import ModeEditOutlineIcon from '@mui/icons-material/ModeEditOutline';
import DeleteIcon from '@mui/icons-material/Delete';
import EditModal from "./editModal";
import CreateProductModal from "./createModal";
import DeleteProductModal from "./deleteModal";
import { useSearchParams } from 'next/navigation'
import { useRouter } from 'next/router'

export interface IProduct {
    id?: string
    name: string
    description?: string
    categoryId?: string
    category: ICategory
}


const Products = () => {
    const connectionService = useContext(ConnectionServiceContext)
    const [categories, setCategories] = useState<ICategory[]>([])
    const [products, setProducts] = useState<IProduct[]>([])

    const searchParams = useSearchParams()
    const categoryFilter = searchParams.get('category')

    useEffect(() => {
        readCategories()
        readProducts()
        
        window.addEventListener('update-categories', readCategories)
        window.addEventListener('update-categories', readProducts)
        window.addEventListener('update-products', readProducts)

        return () => {
            window.removeEventListener('update-categories', readCategories)
            window.removeEventListener('update-categories', readProducts)
            window.removeEventListener('update-products', readProducts)
        }

    }, [])
    
    const router = useRouter()

    const goToMovimentations = (product: IProduct) => {
        router.push(`/movimentations?product=${product.id}`);
    }

    const readProducts = async () => {
        try {
            const _products = (await connectionService?.makeRequest<IProduct[]>('products', 'get'))?.filter(product => product.categoryId === categoryFilter || !categoryFilter)
            _products && setProducts(_products)
        } catch (error) {
            console.log('Error reading products:', error)
        }

    }

    const readCategories = async () => {
        const _categories = await connectionService?.makeRequest<ICategory[]>('categories', 'get')
        _categories && setCategories(_categories)
    }

    let openEditModal: (product: IProduct) => void
    const editProduct = async (product: IProduct) => {
        try {
            product.categoryId = product.category.id
            await connectionService?.makeRequest<IProduct>('products/'+product.id, 'patch', JSON.stringify({...product}))
        } catch (error) {
            console.log({error})
        }
    }

    let openCreateModal: () => void
    const createProduct = async (product: IProduct) => {
        try {
            product.categoryId = product.category.id
            await connectionService?.makeRequest<IProduct>('products', 'post', JSON.stringify({...product}))
        } catch (error) {
            console.log({error})
        }
    }

    let openDeleteModal: (product: IProduct) => void
    const deleteProduct = async (product: IProduct) => {
        try {
            await connectionService?.makeRequest<ICategory>('products/'+product.id, 'delete')
        } catch (error) {
            console.log({error})
        }
    }

    const columns: GridColDef[] = [
        {field: 'categoryName', headerName: 'Category', minWidth: 300},
        {field: 'name', headerName: 'Product', minWidth: 300, type: 'custom', renderCell: (params) => {
            return <span className='cursor-pointer' onClick={e => goToMovimentations(params.row as IProduct)}>{params.row.name}</span>
        }},  
        {field: 'buttons', headerName: 'Buttons', minWidth: 200, type: 'actions',
        getActions: (product) => {
            return [
                <Button variant="outlined" onClick={() => openEditModal(product.row)}><ModeEditOutlineIcon /></Button>,
                <Button onClick={() => openDeleteModal(product.row)}  variant="outlined"><DeleteIcon /></Button>                
            ]
        }}
    ]
    
    return <Container>
        <Button className='float-right'  onClick={() => openCreateModal()} variant="outlined"><AddIcon /></Button>
        <div className='flex items-center justify-center'>
            <div className='w-8/12 md:w-12/12'>
                {(!!products?.length) && <DataGrid columns={columns} rows={products} />}
            </div>
        </div>
        <DeleteProductModal setOpenDeleteModalFn={fn => openDeleteModal = fn}  handleDelete={product => {deleteProduct(product)}}  />
        <EditModal categories={categories} setOpenEditModalFn={fn => openEditModal = fn} handleEdit={product => editProduct(product)}/>
        <CreateProductModal categories={categories} setOpenCreateModalFn={fn => openCreateModal = fn} handleCreate={product => createProduct(product)}  />
    </Container>
}

export default Products