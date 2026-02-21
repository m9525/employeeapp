import { useEffect, useState } from 'react';
import { Employee } from './Employee';

function App() {
    const [employees, setEmployees] = useState<Employee[]>([]);    
    const [selectedEmp, setSelectedEmployee] = useState<Employee>();    
    const maxABCLimit: number = 11171;

    useEffect(() => {
        fetchEmployees();
    }, []);

    async function fetchEmployees() {
        const response = await fetch('api/employees');
        const data = await response.json();
        setEmployees(data);
    }

    const onDelete = async (id: number) => {
        if (!confirm('Delete id ' + id + '?')) return
        await fetch(`/api/employees/${id}`, { method: 'DELETE' })
        fetchEmployees()
    }

    const onIncrease = async () => {        
        await fetch(`/api/employees/increase`, { method: 'GET' })
        fetchEmployees()
    }

    const onAdd = async () => { // TODO
        await fetch(`/api/employees/add`, { method: 'POST' })
        fetchEmployees()
    }

    const onEdit = async () => { // TODO
        await fetch(`/api/employees/update`, { method: 'PUT' })
        fetchEmployees()
    }    
    
    const sumABC = employees.filter((e) => e.name.startsWith("A") || e.name.startsWith("B") || e.name.startsWith("C")).reduce((prev, curr) => prev + curr.value, 0);

    return (<>
        <div>Connectivity check: {employees.length > 0 ? `OK (${employees.length})` : `NOT READY`}</div>
        <div>
            <table>
                {employees.map(e => (
                    <tr>
                        <td><button>Edit</button>
                            <button onClick={() => onDelete(e.id)}>Delete</button></td><td key={e.id}>{e.name}</td><td>{e.value}</td>
                    </tr>
            ))}
            </table>
        </div>
        <div><button onClick={() => onAdd()}>Add Me! TODO</button>
            <div id="newName">NewName</div><div id="newValue">0</div>
        </div>
        <div>
            <button onClick={() => onIncrease()}>Increase Me!</button>
            {
                sumABC <= maxABCLimit ? `` : `Bigger than eq ${maxABCLimit}: ${sumABC}`
            }                
        </div>
    </>);

    
}

export default App;