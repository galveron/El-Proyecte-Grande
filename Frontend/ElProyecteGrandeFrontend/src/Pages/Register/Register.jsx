import { Link } from "react-router-dom"

function RegisterAs() {
    return (
        <div className="register-button-container">
            <button>
                <Link to='/register-customer'>Register as customer</Link>
            </button>
            <button>
                <Link to='/register-company'>Register as company</Link>
            </button>
        </div>
    )
}

export default RegisterAs;