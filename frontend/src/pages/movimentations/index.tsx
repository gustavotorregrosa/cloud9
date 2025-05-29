import { Button, Container } from "@mui/material"
import MovimentationsChart, { ISerieItem } from "@/components/MovimentationsChart"
import { IProduct } from "../products"
import { useContext, useState } from "react"
import { useSearchParams } from "next/navigation"
import { ConnectionServiceContext } from "@/context/ConnectionContext"
import AddIcon from '@mui/icons-material/Add'


const Movimentations = () => {

    const connectionService = useContext(ConnectionServiceContext)
    const searchParams = useSearchParams()
    const productId = searchParams.get('product')
  
    const [product, setProduct] = useState<IProduct>()
    const [series, setSeries] = useState<ISerieItem[]>([])

    const readMovimentations = async () => {
        if (!productId) return

        const _product = await connectionService?.makeRequest<IProduct>(`products/${productId}`, 'get')
        setProduct(_product as IProduct)

        const _series: ISerieItem[] = (await connectionService?.makeRequest<ISerieItem[]>(`movimentations/product/${productId}`, 'get')) || []
        setSeries(_series)
    }

    let openAddMovimentationModal = () => {}

    // const series: ISerieItem[] = [
    //     { xData: new Date(2024, 0, 1), yData: 10 },
    //     { xData: new Date(2024, 0, 2), yData: 15 },
    //     { xData: new Date(2024, 0, 3), yData: 8 },
    //     { xData: new Date(2024, 0, 4), yData: 20 },
    //     { xData: new Date(2024, 0, 5), yData: 12 },
    // ]

    return <Container>
        <Button className='float-right' onClick={() => openAddMovimentationModal()} variant="outlined"><AddIcon /></Button>  
        <MovimentationsChart series={series} />
    </Container>
}

export default Movimentations