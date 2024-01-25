import { Container } from 'reactstrap';
import  NavMenu  from './NavMenu';

function Layout(props){
    
    return (
      <div>
        <NavMenu />
        <Container tag="main">
          {props.children}
        </Container>
      </div>
    );
}

export default Layout;
