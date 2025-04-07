const express = require('express');
const router = express.Router();

// Route for home page
router.get('/', (req, res) => {
  res.render('index'); // Loads the welcome landing page
});

module.exports = router;
