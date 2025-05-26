import { IUser } from "@/store/user/user.interface";
import { login } from "@/store/user/user.slice";
import { Avatar, CardHeader, Container, IconButton } from "@mui/material"
import Card from '@mui/material/Card';
import { useDispatch } from "react-redux";

export const LoginOptions = () => {

    const users: IUser[] = [
        {
            id: '1',
            email: 'dina@gmail.com',
            image: 'dina.png',
            name: 'Dina Fox',
            access_refresh_token: '123',
            access_token: '123',
            password: 'cloud9'
        },

        {
            id: '2',
            email: 'amy@gmail.com',
            image: 'amy.png',
            name: 'Amy Sosa',
            access_refresh_token: '123',
            access_token: '123',
            password: 'cloud9'
        },                                      

        {
            id: '3',
            email: 'jonah@gmail.com',
            image: 'jonah.png',
            name: 'Jonah Simms',
            access_refresh_token: '123',
            access_token: '123',
            password: 'cloud9'
        },

        {
            id: '4',
            email: 'cheyenne@gmail.com',
            image: 'cheyenne.png',
            name: 'Cheyenne Lee',
            access_refresh_token: '123',
            access_token: '123',
            password: 'cloud9'
        },

        {
            id: '5',
            email: 'glenn@gmail.com',
            image: 'glenn.png',
            name: 'Glenn Sturgis',
            access_refresh_token: '123',
            access_token: '123',
            password: 'cloud9'
        },

        {
            id: '6',
            email: 'garret@gmail.com',
            image: 'garret.png',
            name: 'Garret McNeill',
            access_refresh_token: '123',
            access_token: '123',
            password: 'cloud9'
        },

        {
            id: '7',
            email: 'mateo@gmail.com',
            image: 'mateo.png',
            name: 'Mateo Liwanag',
            access_refresh_token: '123',
            access_token: '123',
            password: 'cloud9'
        },
    ]

    const dispatch = useDispatch()

    const loginOnAPI = async (user: IUser) => {
        const result = await fetch(process.env.NEXT_PUBLIC_API_URL+'/Authentication/Login', {
            method: 'post',
            body: JSON.stringify({
                email: user.email,
                password: user.password
            }),
            headers: {
                "Content-Type": "application/json",
            }
        })

        const userResponse: any = await result.json()
        const _user: IUser = {
            ...user,
            access_token: userResponse.accessToken,
            access_refresh_token: userResponse.refreshToken
        }
        // const _user: IUser = await result.json()
        dispatch(login(_user))
        localStorage.setItem('user', JSON.stringify(_user))
    }

    return <Container>
        <div className='flex items-center justify-center flex-wrap '>
                {users.map(user => <Card className="m-5 max-w-128 hover:cursor-pointer" onClick={() => loginOnAPI(user)}  >
                    <CardHeader
                        avatar={<Avatar aria-label={user.name} src={`/images/users/${user.image}`}></Avatar>}
                        title={user.name}
                        subheader={user.email}
                    />
                </Card> )}
        </div>
    </Container>
}