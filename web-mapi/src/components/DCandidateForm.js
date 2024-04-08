import React, { useEffect } from "react";
import {
  Button,
  FormControl,
  FormHelperText,
  Grid,
  InputLabel,
  MenuItem,
  Select,
  TextField,
  Paper,
  withStyles,
} from "@material-ui/core";
import useForm from "./useForm";
import { connect } from "react-redux";
import * as actions from "../actions/dCandidate";
import { useToasts } from "react-toast-notifications";

const styles = (theme) => ({
  root: {
    "& .MuiFormControl-root": {
      width: "80%",
      margin: theme.spacing(1),
    },
  },
  paper: {
    margin: theme.spacing(5),
    padding: theme.spacing(3),
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
  },
  formControl: {
    margin: theme.spacing(1),
    minWidth: 230,
  },
  smMargin: {
    margin: theme.spacing(1),
  },
  buttonGroup: {
    display: "flex",
    justifyContent: "flex-end",
    width: "100%",
    marginTop: theme.spacing(2),
  },
});

const initialFieldValues = {
  fullName: "",
  mobile: "",
  email: "",
  age: "",
  bloodGroup: "",
  address: "",
};

const DCandidateForm = ({ classes, ...props }) => {
  const { addToast } = useToasts();
  const validate = (fieldValues = values) => {
    let temp = { ...errors };
    if ("fullName" in fieldValues)
      temp.fullName = fieldValues.fullName ? "" : "This field is required.";
    if ("mobile" in fieldValues)
      temp.mobile = fieldValues.mobile ? "" : "This field is required.";
    if ("bloodGroup" in fieldValues)
      temp.bloodGroup = fieldValues.bloodGroup ? "" : "This field is required.";
    if ("email" in fieldValues)
      temp.email = /^$|.+@.+..+/.test(fieldValues.email)
        ? ""
        : "Email is not valid.";
    setErrors({ ...temp });
    if (fieldValues === values) return Object.values(temp).every((x) => x === "");
  };

  const {
    values,
    setValues,
    errors,
    setErrors,
    handleInputChange,
    resetForm,
  } = useForm(initialFieldValues, validate, props.setCurrentId);

  useEffect(() => {
    if (props.currentId !== 0) {
      setValues({
        ...props.dCandidateList.find((x) => x.id === props.currentId),
      });
      setErrors({});
    }
  }, [props.currentId]);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (validate()) {
      const onSuccess = () => {
        resetForm();
        addToast("Submitted successfully", { appearance: "success" });
      };
      if (props.currentId === 0) props.createDCandidate(values, onSuccess);
      else props.updateDCandidate(props.currentId, values, onSuccess);
    }
  };

  return (
    <Paper className={classes.paper}>
      <form autoComplete="off" noValidate className={classes.root} onSubmit={handleSubmit}>
        <Grid container spacing={2}>
          <Grid item xs={6}>
            <TextField
              name="fullName"
              variant="outlined"
              label="Full Name*"
              value={values.fullName}
              onChange={handleInputChange}
              {...(errors.fullName && {
                error: true,
                helperText: errors.fullName,
              })}
              data-tst-field-text="FullName"
            />
            <TextField
              name="mobile"
              variant="outlined"
              label="Mobile*"
              value={values.mobile}
              onChange={handleInputChange}
              {...(errors.mobile && { error: true, helperText: errors.mobile })}
            />
            <FormControl variant="outlined" className={classes.formControl} {...(errors.bloodGroup && { error: true })}>
              <InputLabel>Blood Group*</InputLabel>
              <Select
                name="bloodGroup"
                value={values.bloodGroup}
                onChange={handleInputChange}
                label="Blood Group*"
              >
                <MenuItem value="">Select Blood Group</MenuItem>
                <MenuItem value="A+">A +ve</MenuItem>
                <MenuItem value="A-">A -ve</MenuItem>
                <MenuItem value="B+">B +ve</MenuItem>
                <MenuItem value="B-">B -ve</MenuItem>
                <MenuItem value="AB+">AB +ve</MenuItem>
                <MenuItem value="AB-">AB -ve</MenuItem>
                <MenuItem value="0+">0 +ve</MenuItem>
                <MenuItem value="0-">0 -ve</MenuItem>
              </Select>
              {errors.bloodGroup && <FormHelperText>{errors.bloodGroup}</FormHelperText>}
            </FormControl>
          </Grid>
          <Grid item xs={6}>
            <TextField
              name="email"
              variant="outlined"
              label="Email"
              value={values.email}
              onChange={handleInputChange}
              {...(errors.email && { error: true, helperText: errors.email })}
            />
            <TextField
              name="age"
              variant="outlined"
              label="Age"
              value={values.age}
              onChange={handleInputChange}
            />
            <TextField
              name="address"
              variant="outlined"
              label="Address"
              value={values.address}
              onChange={handleInputChange}
            />
          </Grid>
          <Grid item xs={12} className={classes.buttonGroup}>
            <Button
              variant="contained"
              color="primary"
              type="submit"
              className={classes.smMargin}
            >
              Submit
            </Button>
            <Button
              variant="contained"
              onClick={resetForm}
              className={classes.smMargin}
            >
              Reset
            </Button>
          </Grid>
        </Grid>
      </form>
    </Paper>
  );
};

const mapStateToProps = (state) => ({
  dCandidateList: state.dCandidate.list,
});

const mapActionToProps = {
  createDCandidate: actions.create,
  updateDCandidate: actions.update,
};

export default connect(mapStateToProps, mapActionToProps)(withStyles(styles)(DCandidateForm));
