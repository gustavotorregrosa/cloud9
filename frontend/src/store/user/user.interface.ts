export interface IUser {
    id: string
    email: string
    name: string
    access_token: string
    access_refresh_token: string
    image: string
    password?: string
}