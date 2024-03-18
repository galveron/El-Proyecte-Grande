import React, { useState } from 'react';

const AddProduct = () => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [images, setImages] = useState([])
    const [name, setName] = useState("")
    const [price, setPrice] = useState(0.0)
    const [details, setDetails] = useState("")
    const [quantity, setQuatnity] = useState(0)
    const [uploading, setUploading] = useState(false);
    const [imageUploading, setImageUploading] = useState(false)
    const [responseOk, setResponseOk] = useState(false)

    const uploadImage = () => {
        setImageUploading(true)
        setImages([...images, selectedFile]);
        setImageUploading(false)
        setResponseOk(false)
    };

    const handleUpload = async () => {
        try {
            setUploading(true);
            const formData = new FormData();
            images.forEach((file, i) => formData.append(`images`, file))
            const response = await fetch(`http://localhost:5036/Product/AddProduct?name=${name}&price=${price}&details=${details}&quantity=${quantity}`, {
                method: 'POST',
                credentials: 'include',
                body: formData,
            });
            console.log("length: " + selectedFile.length)
            if (response.ok) {
                const data = await response.json();
                setResponseOk(true);
                setSelectedFile(null)
                setImages([])
                setName("")
                setPrice(0.0)
                setDetails("")
                setQuatnity(0)
                setUploading(false)
                setImageUploading(false)
                console.log('Product uploaded successfully.');

            } else {
                console.error('Error uploading image:', response.statusText);
                setUploading(false);
            }
        } catch (error) {
            console.error('Error uploading image:', error.message);
        }
    };

    return (
        <div className='imageuploader'>
            <div className='upload-container'>
                <input type='text' onChange={e => setName(e.target.value)} value={name} required />
                <input type='number' onChange={e => setPrice(e.target.value)} value={price} required />
                <input type='text' onChange={e => setDetails(e.target.value)} value={details} required />
                <input type='number' onChange={e => setQuatnity(e.target.value)} value={quantity} required />
                <input type="file" onChange={(e) => { setSelectedFile(e.target.files[0]) }} />
                {images.length != 0 ? images.map(image => <p key={image.name}>{image.name}</p>) : <p></p>}
                <button onClick={uploadImage} disabled={imageUploading}>Upload image</button>
                <button onClick={handleUpload} disabled={uploading}>Upload Product</button>
                {responseOk ? <p>Successfully uploaded!</p> : <p></p>}
            </div>
        </div>
    );
};

export default AddProduct;
