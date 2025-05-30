import { Button, Container } from "@mui/material"
import MovimentationsChart, { ISerieItem } from "@/components/MovimentationsChart"
import { IProduct } from "../products"
import { useContext, useEffect, useState } from "react"
import { useSearchParams } from "next/navigation"
import { ConnectionServiceContext } from "@/context/ConnectionContext"
import AddIcon from '@mui/icons-material/Add'
import RemoveIcon from '@mui/icons-material/Remove';
import AddMovimentatioModal, { IMovimentationType } from "./addMovimentartionModal"


const Movimentations = () => {

    const connectionService = useContext(ConnectionServiceContext)
    const searchParams = useSearchParams()
    const productId = searchParams.get('product')
  
    const [product, setProduct] = useState<IProduct>()
    const [series, setSeries] = useState<ISerieItem[]>([])

    useEffect(() => {
        readMovimentations()
        window.addEventListener('update-movimentations', readMovimentations)
        return () => window.removeEventListener('update-movimentations', readMovimentations)

    }, [productId])

    const readMovimentations = async () => {
        if (!productId) return

        const _product = await connectionService?.makeRequest<IProduct>(`products/${productId}`, 'get')
        setProduct(_product as IProduct)

        const _series: ISerieItem[] = (
            (await connectionService?.makeRequest<ISerieItem[]>(`movimentations/product/${productId}`, 'get')) || []
        ).map(movimentation => {
            const date = new Date(movimentation.atDate)
            return {
                atDate: new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()),
                value: movimentation.value
            }
            // return {
            // atDate: new Date(date.getFullYear(), date.getMonth(), date.getDate()),
            // value: movimentation.value
            // }
        })
        console.log({_series})
        
        setSeries(_series)
    }

    let openAddMovimentationModal = (_movimentationType: IMovimentationType) => {}

    const addMovimentation = async (_amount: number, _movimentationType: IMovimentationType) => {
        try {
            const amount = _movimentationType === IMovimentationType.ADD ? _amount : -_amount
            await connectionService?.makeRequest<ISerieItem>('movimentations', 'post', JSON.stringify({amount, productId}))
        } catch (error) {
            console.log({error})
        }
    }

    // const series: ISerieItem[] = [
    //     { xData: new Date(2024, 0, 1), yData: 10 },
    //     { xData: new Date(2024, 0, 2), yData: 15 },
    //     { xData: new Date(2024, 0, 3), yData: 8 },
    //     { xData: new Date(2024, 0, 4), yData: 20 },
    //     { xData: new Date(2024, 0, 5), yData: 12 },
    // ]

    return <Container>
        <Button className='float-right' style={{
            marginLeft: '10px',
        }} onClick={() => openAddMovimentationModal(IMovimentationType.ADD)} variant="outlined"><AddIcon /></Button> &nbsp;&nbsp;
        <Button className='float-right' onClick={() => openAddMovimentationModal(IMovimentationType.WITHDRAW)} variant="outlined"><RemoveIcon /></Button>  &nbsp;&nbsp;
        <MovimentationsChart series={series} />
        <AddMovimentatioModal setOpenAddMovimentatioModalFn={(fn) => openAddMovimentationModal = fn } handleAdd={addMovimentation} />
    </Container>
}

export default Movimentations