import React, { useState } from 'react';

const ImageUploader = () => {
    const [selectedFile, setSelectedFile] = useState(null);

    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    const handleUpload = async () => {
        try {
            const formData = new FormData();
            formData.append('file', selectedFile);

            const response = await fetch('http://localhost:5036/Image/upload', {
                method: 'POST',
                body: formData,
            });

            if (response.ok) {
                const data = await response.json();
                console.log('Image uploaded successfully. URL:', data.imageUrl);
            } else {
                console.error('Error uploading image:', response.statusText);
            }
        } catch (error) {
            console.error('Error uploading image:', error.message);
        }
    };

    return (
        <div className='imageuploader'>
            <div className='upload-container'>
                <input type="file" onChange={handleFileChange} />
                <button onClick={handleUpload}>Upload Image</button>
            </div>
        </div>
    );
};

export default ImageUploader;
