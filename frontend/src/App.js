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

  const topDonor = donations.length > 0
    ? donations.reduce((max, donor) => (donor.amount > max.amount ? donor : max), donations[0])
    : null;
  
  const totalAmount = donations.reduce((sum, donation) => sum + parseFloat(donation.amount || 0), 0);

  useEffect(() => {loadDonations(); }, []);
  if (loading) return <p>Loading donations...</p>;
  if (error) return <p>{error}</p>;

  return (
          <div className = "container = fluid">
          <h2>Donation Tracker</h2>
          
          <div classname = "form-container">
           <input className="input-field" placeholder="Donor Name" value={form.donorName} 
              onChange={e => setForm({ ...form, donorName: e.target.value })}/>
          
           <input className="input-field" placeholder="Amount" type="number" value={form.amount}
              onChange={e => setForm({ ...form, amount: e.target.value })}/>

          {/* </div><input placeholder="Amount" type="number" onChange={e => setForm({...form, amount: e.target.value})}/> */}
          
          <input className="input-field" type="date" value={form.date || new Date().toISOString().split("T")[0]} 
            onChange={e => setForm({ ...form, date: e.target.value })}/>
          
        
          <button className="submit-button" onClick={submitDonation}>Submit</button>
          </div>

          {/* </div><button onClick={submitDonation}>Submit</button> */}
          <div className="top-donor">
            <h3><b>🎉<u>Top Donor</u>🎉</b></h3>
            <p><b>{topDonor.donorName}</b> donated <b>${parseFloat(topDonor.amount).toFixed(2)}</b> on
             {new Date(topDonor.date).toLocaleDateString()}</p>
          </div>

          <div className="total-donations">
            <h3>Total Amount Collected: {new Intl.NumberFormat("en-CA", {
              style: "currency",currency: "CAD",}).format(totalAmount || 0)}</h3>
          </div>

          <table className="donation-table">
        <thead><tr><th>Name</th><th>Amount</th><th>Date</th></tr></thead>
        <tbody>
          {donations.map(d => (
            <tr key={d.id}>
              <td>{d.donorName}</td><td>${parseFloat(d.amount).toFixed(2)}</td><td>{new Date(d.date).toLocaleDateString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    );
}

export default App;
