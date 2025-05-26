import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import Tooltip from '@mui/material/Tooltip';
import MenuItem from '@mui/material/MenuItem';
import AdbIcon from '@mui/icons-material/Adb';
import { useRouter } from 'next/router'
import ThunderstormIcon from '@mui/icons-material/Thunderstorm'
import { useSelector, useDispatch } from 'react-redux';
import { IState } from '@/store';
import { IUser } from '@/store/user/user.interface';
import { logout } from '@/store/user/user.slice';
import CampaignIcon from '@mui/icons-material/Campaign';

interface AppBarProps {
  openSendMessageModal: () => void
}


function ResponsiveAppBar({openSendMessageModal}: AppBarProps) {

  const pages: {
    name: string;
    page: string;
  }[] = [
    {
      name: 'Categories',
      page: 'categories'
    },
    {
      name: 'Products',
      page: 'products'
    },
    // {
    //   name: 'Alert',
    //   page: 'alerts'
    // },
  
  ]

  const user = useSelector<IState, IUser>(state => state.user)

  React.useEffect(() => {
    !user.id && router.push('/')

  }, [user])

  const dispatch = useDispatch()
  
  const settings = [
    {
      label: 'Logout',
      fn: () => dispatch(logout())
    }
  ];

  const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);

  const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const router = useRouter()

  return (
    <AppBar position="static">
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          {/* <AdbIcon sx={{ display: { xs: 'none', md: 'flex' }, mr: 1 }} /> */}
          <ThunderstormIcon sx={{ display: { xs: 'none', md: 'flex' }, mr: 1 }} />
          
          <Typography
            onClick={() => {
              handleCloseNavMenu()
              router.push('/')
            }}
            style={{
              cursor: 'pointer'
            }}
            variant="h6"
            noWrap
            component="a"
            // href="#app-bar-with-responsive-menu"
            sx={{
              mr: 2,
              display: { xs: 'none', md: 'flex' },
              fontFamily: 'monospace',
              fontWeight: 700,
              letterSpacing: '.3rem',
              color: 'inherit',
              textDecoration: 'none',
            }}
          >
             Cloud9
          </Typography>
        

          <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'left',
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: 'block', md: 'none' },
              }}
            >
              {user.id && pages.map((page) => (
                 <MenuItem key={page.page} onClick={() => {
                    handleCloseNavMenu()
                    router.push(page.page)
                  }}>
                  <Typography textAlign="center">{page.name}</Typography>
                </MenuItem>   
              ))}
            </Menu>
          </Box>

          <ThunderstormIcon sx={{ display: { xs: 'flex', md: 'none' }, mr: 1 }} />
          {/* <AdbIcon sx={{ display: { xs: 'flex', md: 'none' }, mr: 1 }} /> */}
          
          <Typography
            style={{
              cursor: 'pointer'
            }}
            variant="h5"
            noWrap
            component="a"
            // href="#app-bar-with-responsive-menu"
            sx={{
              mr: 2,
              display: { xs: 'flex', md: 'none' },
              flexGrow: 1,
              fontFamily: 'monospace',
              fontWeight: 700,
              letterSpacing: '.3rem',
              color: 'inherit',
              textDecoration: 'none',
            }}
          >
            Cloud9
          </Typography>
          <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
            {user.id && pages.map((page) => (
                <Button
                  key={page.page}
                  onClick={() => {
                    handleCloseNavMenu()
                    router.push(page.page)
                  }}
                  sx={{ my: 2, color: 'white', display: 'block' }}
                >
                {page.name}
              </Button>
            ))}
          </Box>

          <CampaignIcon onClick={() => openSendMessageModal()} style={{
            cursor: 'pointer'
          }} /> &nbsp; &nbsp; 

          {user.id && 
          <Box sx={{ flexGrow: 0 }}>
            <Tooltip title="Open settings">
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <Avatar alt="Gemy Sharp" src={`/images/users/${user.image}`} />
              </IconButton>
            </Tooltip>
            <Menu
              sx={{ mt: '45px' }}
              id="menu-appbar"
              anchorEl={anchorElUser}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              open={Boolean(anchorElUser)}
              onClose={handleCloseUserMenu}
            >
              {settings.map((setting) => (
                <MenuItem key={setting.label} onClick={() => {
                  handleCloseUserMenu()
                  setting.fn()
                }}>
                  <Typography textAlign="center">{setting.label}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>}

        </Toolbar>
      </Container>
    </AppBar>
  );
}
export default ResponsiveAppBar;
