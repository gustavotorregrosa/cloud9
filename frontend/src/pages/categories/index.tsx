import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Container } from '@mui/material';
import Button from '@mui/material/Button';
import ModeEditOutlineIcon from '@mui/icons-material/ModeEditOutline';
import DeleteIcon from '@mui/icons-material/Delete';
import EditModal from './editModal'
import DeleteModal from './deleteModal'
import CreateCategoryModal from './createModal';
import AddIcon from '@mui/icons-material/Add';
import { useContext, useEffect, useState } from 'react';
import { ConnectionServiceContext } from '@/context/ConnectionContext';

export interface ICategory {
    id: string
    name: string
}

const Categories = () => {

    const connectionService = useContext(ConnectionServiceContext)
    const [categories, setCategories] = useState<ICategory[]>()
    const readCategories = async () => {
        const _categories = await connectionService?.makeRequest<ICategory[]>('categories', 'get')
        _categories && setCategories(_categories)
    }

    useEffect(() => {
        readCategories()
    }, [])

    useEffect(() => {
        window.addEventListener('update-categories', readCategories)
        return () => window.removeEventListener('update-categories', readCategories)
    }, [categories])


    let openEditModal: (category: ICategory) => void
    const editCategory = async (category: ICategory) => {
        try {
            await connectionService?.makeRequest<ICategory>('categories/'+category.id, 'put', JSON.stringify({name: category.name}))
        } catch (error) {
            console.log({error})
        }
    
    }

    let openDeleteModal: (category: ICategory) => void
    const deleteCategory = async (category: ICategory) => {
        try {
            await connectionService?.makeRequest<ICategory>('categories/'+category.id, 'delete')
        } catch (error) {
            console.log({error})
        }

    }

    let openCreateModal: () => void
    const createCategory = async (name: string) => {
        try {
            await connectionService?.makeRequest<ICategory>('categories', 'post', JSON.stringify({name}))
        } catch (error) {
            console.log({error})
        }
    }

    const columns: GridColDef[] = [
        {field: 'name', headerName: 'Category', minWidth: 400 },
        {field: 'buttons', headerName: 'Buttons', minWidth: 400, type: 'actions',
        getActions: (category) => {
            return [
                <Button variant="outlined" onClick={() => openEditModal(category.row)}><ModeEditOutlineIcon /></Button>,
                <Button onClick={() => openDeleteModal(category.row)} variant="outlined"><DeleteIcon /></Button>                
            ]
        }}
    ]

    return <Container>
        <Button className='float-right' onClick={() => openCreateModal()} variant="outlined"><AddIcon /></Button>  
        <div className='flex items-center justify-center'>
            <div className='w-8/12 md:w-12/12'>
                {(!!categories?.length) && <DataGrid  columns={columns} rows={categories} />}
            </div>
        </div>
        <EditModal setOpenEditModalFn={fn => openEditModal = fn} handleEdit={category => editCategory(category)}/>
        <DeleteModal setOpenDeleteModalFn={fn => openDeleteModal = fn}  handleDelete={category => {deleteCategory(category)}}  />
        <CreateCategoryModal setOpenCreateModalFn={fn => openCreateModal = fn} handleCreate={name => createCategory(name)}  />
    </Container>
}

export default Categories