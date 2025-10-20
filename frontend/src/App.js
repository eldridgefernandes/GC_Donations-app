import React, { useState, useEffect } from "react";
import axios from "axios";
import './App.css';

function App() {
  const [donations, setDonations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [anonymous, setAnonymous] = useState(false);
  const [form, setForm] = useState({date: "", donorfirstname: "", donorlastname: "",elmail: "",phonenumber: "",amount: "",
    projectpreference: "",donationmethod: "", frequency: "",address: "",city: "", province: "",postalcode: ""});

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
      
      const payload = {
        ...form,
        donorfirstname: anonymous ? "Anonymous" : form.donorfirstname,
        donorlastname: anonymous ? "" : form.donorlastname,
        elmail: anonymous ? "" : form.elmail,
        date: form.date && form.date.trim() !== "" ? new Date(form.date).toISOString() : new Date().toISOString(),
      };
      
      await axios.post("http://localhost:5050/api/donations", payload);
      setForm({ 
      //  date: form.date || new Date().toISOString(),
       date: form.date && form.date.trim() !== "" 
        ? new Date(form.date).toISOString() 
        : new Date().toISOString().split("T")[0],
       donorfirstname: form.donorfirstname,
       donorlastname : form.donorlastname ,
       elmail : form.elmail ,
       phonenumber : form.phonenumber,
       amount: parseFloat(form.amount),
       projectpreference : form.projectpreference,
       donationmethod : form.donationmethod,
       frequency : form.frequency,
       address : form.address,
       city : form.city,
       province : form.province,
       postalcode : form.postalcode,

      });
      setAnonymous(false);
      await loadDonations();
      alert("✅ Donation completed and records updated successfully!");
    }
    catch(error){
      console.error("Error submitting donation:", error);
      alert("❌ Error: Unable to complete the donation. Please try again.");
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
          <div className = "container">
          <h2>Donation Tracker</h2>
          
          <div className = "form-container">
           
          <div className="first-line">
           <label><input type="checkbox" checked={anonymous} onChange={() => setAnonymous(!anonymous)}/> Donate Anonymously
           </label>
          </div>
          
           {!anonymous && (
          <div className = "secondary-fields">
            <input className="input-field" placeholder="Donor First Name" value={form.donorfirstname}
              onChange={e => setForm({ ...form, donorfirstname: e.target.value })}/>

            <input className="input-field" placeholder="Donor Last Name" value={form.donorlastname}
              onChange={e => setForm({ ...form, donorlastname: e.target.value })}/>

            <input className="input-field" type="email" placeholder="Email" value={form.elmail}
              onChange={e => setForm({ ...form, elmail: e.target.value })} required/>
                {/* Show error message if email is invalid */}
              {form.elmail && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.elmail) && (
                <p className="error-text">Please enter a valid email address.</p>)}

            <input className="input-field" placeholder="Phone_Number" value={form.phonenumber} 
              onChange={e => setForm({ ...form, phonenumber: e.target.value })}/>
            
            <input className="input-field" placeholder="Address" value={form.address} 
              onChange={e => setForm({ ...form, address: e.target.value })}/>

            <input className="input-field" placeholder="City" value={form.city} 
              onChange={e => setForm({ ...form, city: e.target.value })}/>

            <input className="input-field" placeholder="Province" value={form.province} 
              onChange={e => setForm({ ...form, province: e.target.value })}/>

             <input className="input-field" placeholder="Postal_code" value={form.postalcode} 
              onChange={e => setForm({ ...form, postalcode: e.target.value })}/>
            
              
          </div>
        )}
           
           {/* <input className="input-field" placeholder="Donor First Name" value={form.donorfirstname} 
              onChange={e => setForm({ ...form, donorfirstname: e.target.value })}/>
          
           <input className="input-field" placeholder="Donor Last Name" value={form.donorlastname} 
              onChange={e => setForm({ ...form, donorlastname: e.target.value })}/> */}
          
           {/* <input className="input-field" placeholder="Email" value={form.elmail} 
              onChange={e => setForm({ ...form, elmail: e.target.value })}/> */}

            {/* <div className="email-field"> */}
              {/* <input className="input-field" type="email" placeholder="Email" value={form.elmail}
                onChange={e => setForm({ ...form, elmail: e.target.value })} required/> */}
                {/* Show error message if email is invalid */}
                {/* {form.elmail && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.elmail) && (
                  <p className="error-text">Please enter a valid email address.</p>)} */}
            {/* </div> */}

            {/* <input className="input-field" placeholder="Phone_Number" value={form.phonenumber} 
              onChange={e => setForm({ ...form, phonenumber: e.target.value })}/> */}

            <input className="input-field" placeholder="Amount" type="number" value={form.amount}
              onChange={e => setForm({ ...form, amount: e.target.value })}/>

            <input className="input-field" placeholder="Project_Preference" value={form.projectpreference} 
              onChange={e => setForm({ ...form, projectpreference: e.target.value })}/>

             <input className="input-field" placeholder="Donation_Method" value={form.donationmethod} 
              onChange={e => setForm({ ...form, donationmethod: e.target.value })}/>
 
             <input className="input-field" placeholder="Frequency" type="number" value={form.frequency} 
              onChange={e => setForm({ ...form, frequency: e.target.value })}/>

            {/* <input className="input-field" placeholder="Address" value={form.address} 
              onChange={e => setForm({ ...form, address: e.target.value })}/> */}

            {/* <input className="input-field" placeholder="City" value={form.city} 
              onChange={e => setForm({ ...form, city: e.target.value })}/> */}

            {/* <input className="input-field" placeholder="Province" value={form.province} 
              onChange={e => setForm({ ...form, province: e.target.value })}/> */}

            {/* <input className="input-field" placeholder="Postal_code" value={form.postalcode} 
              onChange={e => setForm({ ...form, postalcode: e.target.value })}/> */}

          {/* </div><input placeholder="Amount" type="number" onChange={e => setForm({...form, amount: e.target.value})}/> */}
          
          <input className="input-field" type="date" value={form.date ? form.date.split("T")[0]
            : new Date().toISOString().split("T")[0]} 
            onChange={e => setForm({...form,date: e.target.value || new Date().toISOString().split("T")[0],})}/>
          
          <button className="submit-button" onClick={submitDonation}>Submit</button>
          </div>

          {/* </div><button onClick={submitDonation}>Submit</button> */}
          <div className="top-donor">
            <h3><b>🎉<u>Top Donor</u>🎉</b></h3>
            <p><b>{topDonor.donorfirstname} {topDonor.donorlastname} </b>
             donated <b>${parseFloat(topDonor.amount).toFixed(2)}</b> on {new Date(topDonor.date).toLocaleDateString()}</p>
          </div>

          <div className="total-donations">
            <h3>Total Amount Collected: {new Intl.NumberFormat("en-CA", {
              style: "currency",currency: "CAD",}).format(totalAmount || 0)}</h3>
          </div>

          <table className="donation-table">
        <thead><tr><th>First Name</th><th>Last Name</th><th>Email</th><th>Phone Number</th>
        <th>Amount</th><th>Date</th>
        <th>Project Perference</th><th>Donation Method</th><th>Frequency</th><th>Address</th>
        <th>City</th><th>Province</th><th>Postal Code</th></tr></thead>
        <tbody>
          {donations.map(d => (
            <tr key={d.id}>
              <td>{d.donorfirstname}</td><td>{d.donorlastname}</td><td>{d.elmail}</td><td>{d.phonenumber}</td>
              <td>${parseFloat(d.amount).toFixed(2)}</td><td>{new Date(d.date).toLocaleDateString()}</td>
              <td>{d.projectpreference}</td><td>{d.donationmethod}</td><td>{d.frequency}</td><td>{d.address}</td>
              <td>{d.city}</td><td>{d.province}</td><td>{d.postalcode}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    );
}

export default App;
