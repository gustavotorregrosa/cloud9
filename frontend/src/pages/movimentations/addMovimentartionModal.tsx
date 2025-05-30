import { Box, Button } from '@mui/material';
import Modal from '@mui/material/Modal';
import { Input } from '@mui/material';
import SaveAltIcon from '@mui/icons-material/SaveAlt';
import { useState } from 'react';

export enum IMovimentationType {
    ADD = 'ADD',
    WITHDRAW = 'WITHDRAW'
}

interface IAddMovimentationModalProps {
    handleAdd: (amount: number, _movimentationType: IMovimentationType) => void
    setOpenAddMovimentatioModalFn: (fn: (_movimentationType: IMovimentationType) => void) => void
}

const AddMovimentatioModal = ({setOpenAddMovimentatioModalFn, handleAdd}: IAddMovimentationModalProps) => {

    const [amount, setAmount] = useState<number>(1)
    const [open, setOpen] = useState<boolean>(false)
    const [movimentationType, setMovimentationType] = useState<IMovimentationType>(IMovimentationType.ADD)

    const openModal = (_movimentationType: IMovimentationType) => {
        setMovimentationType(_movimentationType)
        setOpen(true)
        setAmount(1)
    }

    // useEffect(() => {
    //     setOpenAddMovimentatioModalFn && setOpenAddMovimentatioModalFn(openModal)
    // }, [])

    setOpenAddMovimentatioModalFn && setOpenAddMovimentatioModalFn(openModal)

    const _handleAdd = () => {
        setOpen(false)
        handleAdd(amount, movimentationType)
        setAmount(1)
        setMovimentationType(IMovimentationType.ADD)
    }

    const handleChange = (_amount: string) => {
        try {
            const parsedAmount = parseInt(_amount)
            if (isNaN(parsedAmount)) {
                setAmount(1)
                return
            }
            setAmount(parsedAmount)
        }catch (error) {
            console.error("Error parsing amount:", error);
            setAmount(1);
        }
    }

    return <Modal
                open={open}
                onClose={() => setOpen(false)}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <p>{movimentationType == IMovimentationType.ADD ? 'Add Item' : 'Withdraw Item'}</p>
                    <Input 
                        type='number' 
                        value={amount} 
                        onChange={e => {
                            const value = e.target.value;
                            if (parseInt(value) > 0) {
                                handleChange(value);
                            }
                        }} 
                    />&nbsp;&nbsp;
                    <Button onClick={() => _handleAdd()} variant="outlined" ><SaveAltIcon /></Button>
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

export default AddMovimentatioModal