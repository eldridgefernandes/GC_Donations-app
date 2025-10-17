import React, { useState, useEffect } from "react";
import axios from "axios";
import './App.css';

function App() {
  const [donations, setDonations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [form, setForm] = useState({ donorName: "", amount: "", date: "" });

 const DonationsTable = () => {
  const [donations, setDonations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
 }
 
  const loadDonations = async () => {
    try{
      const res = await axios.get("http://localhost:5050/api/donations");
      console.log("Fetched donations:", res.data);
      setDonations(res.data);
      setLoading(false);
    }
    
    catch(error){
      console.error("Error loading donations:", error);
      setError("Failed to load donations1");
      setLoading(false);
    }
  };
   
  const submitDonation = async () => {
    try{
      await axios.post("http://localhost:5050/api/donations", form);
      setForm({ 
       donorName: form.donorName,
       amount: parseFloat(form.amount),
       date: form.date || new Date().toISOString()
      });
      await loadDonations();
     
    }
    catch(error){
      console.error("Error submitting donation:", error);
    }
  };

  useEffect(() => {loadDonations(); }, []);
  if (loading) return <p>Loading donations...1</p>;
  if (error) return <p>{error}</p>;

  return (
          <div>
          <h2>Donation Tracker</h2>
          <input placeholder="Donor Name" onChange={e => setForm({...form, donorName: e.target.value})}/>
          <input placeholder="Amount" type="number" onChange={e => setForm({...form, amount: e.target.value})}/>
          <input type="date" onChange={e => setForm({...form, date: e.target.value})}/>
          <button onClick={submitDonation}>Submit</button>

          <table>
          <thead><tr><th>Name</th><th>Amount</th><th>Date</th></tr></thead>
           <tbody>
             {donations.map(d => (
                <tr key={d.id}><td>{d.donorName}</td><td>{d.amount}</td><td>{new Date(d.date).toLocaleDateString()}</td></tr>
              ))}
            </tbody>
          </table>
        </div>
    );
}

export default App;
