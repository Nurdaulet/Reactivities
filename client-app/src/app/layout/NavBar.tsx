
import { observer } from 'mobx-react-lite';
import { createMedia } from '@artsy/fresnel'
import PropTypes from 'prop-types';
import React, { Component, Requireable } from 'react';
import { Link, NavLink } from 'react-router-dom';
import { Button, Container, Dropdown, Image, Menu, Visibility, Sidebar, Segment, Grid, Header, Divider, List, Icon } from 'semantic-ui-react';
import { useStore } from '../stores/store';
import { User } from '../models/user';

interface Props {
  children: React.ReactNode;
}

export default observer(function NavBar({ children }: Props) {
  return (
    <ResponsiveContainer>
      {children}
      {/* <Menu inverted fixed='top'>
        <Container>

          <Menu.Item position='right'>
            <Image src={user?.image || '/assets/user.png'} avatar spaced="right" />
            <Dropdown pointing='top left' text={user?.displayName}>
              <Dropdown.Menu>
                <Dropdown.Item as={Link} to={`/profiles/${user?.username}`} text='My Profile' icon='user' />
                <Dropdown.Item onClick={logOut} text='Logout' icon='power' />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        </Container>
      </Menu> */}
    </ResponsiveContainer>
  )
})


const AppMedia = createMedia({
  breakpoints: {
    mobile: 320,
    tablet: 768,
    computer: 992,
    largeScreen: 1200,
    widescreen: 1920
  }
});

const mediaStyles = AppMedia.createMediaStyle();
const { Media, MediaContextProvider } = AppMedia;

const ResponsiveContainer = ({ children }: Props) => (
  /* Heads up!
   * For large applications it may not be best option to put all page into these containers at
   * they will be rendered twice for SSR.
   */
  <>
    <style>{mediaStyles}</style>
    <MediaContextProvider>
      <NavBarContent>
        {children}
      </NavBarContent>
    </MediaContextProvider>
  </>
)


class NavBarContent extends React.Component {


  state = {
    visible: false
  };

  handlePusher = () => {
    const { visible } = this.state;
    if (visible) this.setState({ visible: false });
  };

  handleToggle = () => this.setState({ visible: !this.state.visible });

  render() {
    const { children } = this.props;
    const { visible } = this.state;

    return (
      <div>
        <Media at="mobile">
          <NavBarMobile
            onPusherClick={this.handlePusher}
            onToggle={this.handleToggle}
            visible={visible}
          >
            <NavBarChildren>{children}</NavBarChildren>
          </NavBarMobile>
        </Media>

        <Media greaterThan="mobile">
          <NavBarDesktop />
          <NavBarChildren>{children}</NavBarChildren>
        </Media>
      </div>
    );
  }
}

interface NavBarMobileProps {
  children: React.ReactNode;
  onPusherClick: () => void,
  onToggle: () => void,
  visible: boolean
}

const NavBarMobile = ({ children, onPusherClick, onToggle, visible }: NavBarMobileProps) => {
  const { userStore: { user, logOut } } = useStore();
  return (
    <Sidebar.Pushable>
      <Sidebar
        as={Menu}
        animation="overlay"
        icon="labeled"
        inverted
        vertical
        visible={visible}
      >
        <Menu.Item as={NavLink} to='/activities' name="Activites" />
        <Menu.Item as={NavLink} to='/items' name="Items" />
        <Menu.Item as={NavLink} to='/errors' name="Errors" />
        <Menu.Item>
          <Button as={NavLink} to='/createActivity' positive content="Create Activity" />
        </Menu.Item>

        <Menu.Item as={Link} to={`/profiles/${user?.username}`} name='My Profile' />
        <Menu.Item onClick={logOut} name='Logout' />
      </Sidebar>
      <Sidebar.Pusher
        dimmed={visible}
        onClick={onPusherClick}
        style={{ minHeight: "100vh" }}
      >
        <Menu fixed="top" inverted>
          <Menu.Item as={NavLink} to='/' exact header>
            <img src="/assets/logo.png" alt="logo" style={{ marginRight: "10px" }} />
            Reactivities
          </Menu.Item>
          <Menu.Item onClick={onToggle}>
            <Icon name="sidebar" />
          </Menu.Item>
          <Menu.Item position="right">
            <Image src={user?.image || '/assets/user.png'} avatar spaced="right" />
          </Menu.Item>
        </Menu>
        {children}
      </Sidebar.Pusher>
    </Sidebar.Pushable>
  );
};

const NavBarChildren = ({ children }: Props) => (
  <Container style={{ marginTop: "5em" }}>{children}</Container>
);


const NavBarDesktop = () => {
  const { userStore: { user, logOut } } = useStore();
  return (
    <Segment
      inverted
      textAlign='center'
      vertical>
      <Menu fixed="top" inverted size='large'>
        <Container>
          <Menu.Item as={NavLink} to='/' exact header>
            <img src="/assets/logo.png" alt="logo" style={{ marginRight: "10px" }} />
            Reactivities
          </Menu.Item>
          <Menu.Item as={NavLink} to='/activities' name="Activites" />
          <Menu.Item as={NavLink} to='/items' name="Items" />
          <Menu.Item as={NavLink} to='/errors' name="Errors" />
          <Menu.Item>
            <Button as={NavLink} to='/createActivity' positive content="Create Activity" />
          </Menu.Item>

          <Menu.Item position="right">
            <Image src={user?.image || '/assets/user.png'} avatar spaced="right" />
            <Dropdown pointing='top left' text={user?.displayName}>
              <Dropdown.Menu>
                <Dropdown.Item as={Link} to={`/profiles/${user?.username}`} text='My Profile' icon='user' />
                <Dropdown.Item onClick={logOut} text='Logout' icon='power' />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        </Container>
      </Menu>
    </Segment>
  );
};