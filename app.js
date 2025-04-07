// Required dependencies
const express = require('express');
const path = require('path');
const logger = require('morgan');
const cookieParser = require('cookie-parser');
const session = require('express-session');
const passport = require('passport');
const mongoose = require('mongoose');
const MongoStore = require('connect-mongo');
require('dotenv').config(); // Load environment variables

// Route imports
const indexRouter = require('./routes/index');
const notesRouter = require('./routes/notes');
const authRouter = require('./routes/auth');

// Initialize express app
const app = express();

// Set view engine and views directory
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'hbs');

// Middleware for logging, parsing requests and serving static files
app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public'))); // Serve static files
app.use('/uploads', express.static(path.join(__dirname, 'public/uploads'))); // Serve uploads folder

// Setup express-session and store in MongoDB
app.use(
  session({
    secret: process.env.SESSION_SECRET || 'MyPassword1234',
    resave: false,
    saveUninitialized: false,
    store: MongoStore.create({
      mongoUrl: process.env.MONGO_URI, // MongoDB connection from .env
    }),
  })
);

// Initialize passport and session
require('./config/passport')(passport);
app.use(passport.initialize());
app.use(passport.session());

// Main routes for the app
app.use('/', indexRouter);
app.use('/notes', notesRouter);
app.use('/auth', authRouter);

// Handle 404 - page not found
app.use((req, res, next) => {
  const err = new Error('Not Found');
  err.status = 404;
  next(err);
});

// Handle other errors
app.use((err, req, res, next) => {
  res.status(err.status || 500);
  res.render('error', {
    message: err.message,
    error: req.app.get('env') === 'development' ? err : {}, // Only show stack trace in dev
  });
});

// Connect to MongoDB with Mongoose
mongoose
  .connect(process.env.MONGO_URI)
  .then(() => console.log('MongoDB connected')) // Log successful connection
  .catch((err) => console.error('MongoDB connection error:', err)); // Log errors

// Export the app
module.exports = app;
