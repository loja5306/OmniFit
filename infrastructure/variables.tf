variable "aws_region" {
  type = string
  default = "eu-west-2"
}

variable "ec2_public_key" {
  type        = string
  sensitive   = true
}

variable "cloudflare_api_token" {
  type = string
  sensitive = true
}

variable "frontend_domain" {
  type = string
  default = "omnifit.lukeatkinson.dev"
}