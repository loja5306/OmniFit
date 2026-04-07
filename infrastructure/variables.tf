variable "aws_region" {
  type = string
  default = "eu-west-2"
}

variable "ec2_public_key" {
  type        = string
  sensitive   = true
}